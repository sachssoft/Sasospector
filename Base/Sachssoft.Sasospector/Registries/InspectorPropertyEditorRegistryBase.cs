using Sachssoft.Sasospector.Purposes;
using Sachssoft.Sasospector.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sachssoft.Sasospector.Registries
{
    public abstract class InspectorPropertyEditorRegistryBase
    {
        private readonly IInspectorEditorPlatformFactory _platformFactory;
        private readonly Dictionary<Type, List<Entry>> _entries = new();
        private readonly List<RuleEntry> _rules = new();
        private readonly List<RedirectionEntry> _redirections = new();
        private readonly HashSet<IInspectorPropertyEditorModule> _registeredModules = new();

        private sealed record Entry(
            Func<IInspectorEditorPlatformFactory, IPropertyEditor> Factory,
            IInspectorPropertyPurpose? Purpose,
            bool IsFallback
        );

        private sealed record RuleEntry(
            Func<Type, bool> Match,
            Func<IInspectorPropertyInfo, IInspectorEditorPlatformFactory, IPropertyEditor> Factory,
            IInspectorPropertyPurpose? Purpose,
            bool IsFallback
        );

        private sealed record RedirectionEntry(
            Func<Type, bool> Match,
            Func<Type, Type> Redirect,
            IInspectorPropertyPurpose? Purpose,
            bool IsFallback
        );

        public InspectorPropertyEditorRegistryBase()
        {
            _platformFactory = CreatePlatformFactory();
        }

        public InspectorPropertyEditorRegistryBase(IEnumerable<IInspectorPropertyEditorModule> modules)
        {
            _platformFactory = CreatePlatformFactory();

            foreach (var module in modules)
                AddModule(module);
        }

        public void RegisterType(
            Type type,
            Func<IInspectorEditorPlatformFactory, IPropertyEditor> factory,
            IInspectorPropertyPurpose? purpose = null,
            bool isFallback = false)
        {
            var list = GetOrCreateList(type);
            list.Add(new Entry(factory, purpose, isFallback));
        }

        public void RegisterRule(
            Func<Type, bool> match,
            Func<IInspectorPropertyInfo, IInspectorEditorPlatformFactory, IPropertyEditor> factory,
            IInspectorPropertyPurpose? purpose = null,
            bool isFallback = false)
        {
            _rules.Add(new RuleEntry(match, factory, purpose, isFallback));
        }

        // Weiterleitung von generischen Typen, z.B. Nullable<T> -> T, Array<T> -> T, ...
        // z.b. Nullable<T>, Array<T>, ...
        public void RegisterRedirection(
            Func<Type, bool> match,
            Func<Type, Type> target,
            IInspectorPropertyPurpose? purpose = null,
            bool isFallback = false)
        {
            _redirections.Add(new RedirectionEntry(match, target, purpose, isFallback));
        }

        public bool TryAddModule(IInspectorPropertyEditorModule module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            if (!_registeredModules.Add(module))
                return false;

            module.Register(this);
            return true;
        }

        public void AddModule(IInspectorPropertyEditorModule module)
        {
            if (!TryAddModule(module))
            {
                throw new InvalidOperationException(
                    $"Inspector module '{module.GetType().FullName}' is already registered.");
            }
        }

        public IPropertyEditor? Create(IInspectorPropertyInfo info, string? preferredEditorKind = null)
        {
            var type = ResolveType(info.Type);
            var purpose = info.Metadata.Purpose;

            IPropertyEditor? result = null;

            // 1. EXACT TYPE MATCH
            if (_entries.TryGetValue(type, out var exactList))
            {
                var filtered = exactList
                    .Where(e => e.Purpose == null || e.Purpose.IsEquivalentTo(purpose))
                    .ToList();

                result =
                    filtered.FirstOrDefault(e => !e.IsFallback)?.Factory(_platformFactory)
                    ?? filtered.FirstOrDefault(e => e.IsFallback)?.Factory(_platformFactory);
            }

            // 2. ASSIGNABLE MATCH
            if (result == null)
            {
                var candidates = _entries
                    .Where(x => x.Key.IsAssignableFrom(type))
                    .SelectMany(x => x.Value)
                    .Where(e => e.Purpose == null || e.Purpose.IsEquivalentTo(purpose))
                    .ToList();

                result =
                    candidates.FirstOrDefault(e => !e.IsFallback)?.Factory(_platformFactory)
                    ?? candidates.FirstOrDefault(e => e.IsFallback)?.Factory(_platformFactory);
            }

            // 3. RULE SYSTEM
            if (result == null)
            {
                var rules = _rules
                    .Where(r => r.Match(type))
                    .Where(r => r.Purpose == null || r.Purpose.IsEquivalentTo(purpose))
                    .ToList();

                result =
                    rules.FirstOrDefault(r => !r.IsFallback)?.Factory(info, _platformFactory)
                    ?? rules.FirstOrDefault(r => r.IsFallback)?.Factory(info, _platformFactory);
            }

            // ❗ FINAL SAFETY: wirklich nichts gefunden → NULL
            if (result == null)
                return null;

            result.PreferredKind = preferredEditorKind;
            return result;
        }

        protected abstract IInspectorEditorPlatformFactory CreatePlatformFactory();

        private Type ResolveType(Type type, HashSet<Type>? visited = null)
        {
            visited ??= new HashSet<Type>();

            if (!visited.Add(type))
                return type; // Schutz vor Zyklen

            var redirection = _redirections
                .Where(r => r.Match(type))
                .FirstOrDefault();

            if (redirection == null)
                return type;

            var redirected = redirection.Redirect(type);

            if (redirected == type || redirected == null)
                return type;

            return ResolveType(redirected, visited);
        }

        private List<Entry> GetOrCreateList(Type type)
        {
            if (!_entries.TryGetValue(type, out var list))
            {
                list = new List<Entry>();
                _entries[type] = list;
            }

            return list;
        }
    }
}