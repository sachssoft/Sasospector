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
            int Priority
        );

        private sealed record RuleEntry(
            Func<Type, bool> Match,
            Func<IInspectorPropertyInfo, IInspectorEditorPlatformFactory, IPropertyEditor> Factory,
            int Priority
        );

        private sealed record RedirectionEntry(
            Func<Type, bool> Match,
            Func<Type, Type> Redirect,
            Func<IEnumerable<IInspectorConstraint>, bool>? ConstraintMatch,
            int Priority
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
            Func<IEnumerable<IInspectorConstraint>, bool>? constraintMatch = null,
            int priority = 0)
        {
            var list = GetOrCreateList(type);
            list.Add(new Entry(factory, priority));
        }

        public void RegisterRule(
            Func<Type, bool> match,
            Func<IInspectorPropertyInfo, IInspectorEditorPlatformFactory, IPropertyEditor> factory,
            Func<IEnumerable<IInspectorConstraint>, bool>? constraintMatch = null,
            int priority = 0)
        {
            _rules.Add(new RuleEntry(match, factory, priority));
        }

        // Weiterleitung von generischen Typen, z.B. Nullable<T> -> T, Array<T> -> T, ...
        // z.b. Nullable<T>, Array<T>, ...
        public void RegisterRedirection(
            Func<Type, bool> match,
            Func<Type, Type> target,
            Func<IEnumerable<IInspectorConstraint>, bool>? constraintMatch = null,
            int priority = 0)
        {
            _redirections.Add(new RedirectionEntry(match, target, constraintMatch, priority));
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
            IPropertyEditor? result = null;

            var type = ResolveType(info.Type);

            // 1. RULE SYSTEM
            var rule = _rules
                .Where(r => r.Match(type))
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault();

            if (rule != null)
            {
                result = rule.Factory(info, _platformFactory);
            }
            else
            {
                // 2. REGISTRY SYSTEM
                var candidates = _entries
                    .Where(x => x.Key.IsAssignableFrom(type))
                    .SelectMany(x => x.Value)
                    .ToList();

                if (candidates.Count == 0)
                    return null;

                result = candidates
                    .OrderByDescending(x => x.Priority)
                    .First()
                    .Factory(_platformFactory);
            }

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
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault();

            if (redirection == null)
                return type;

            var redirected = redirection.Redirect(type);

            if (redirected == type)
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