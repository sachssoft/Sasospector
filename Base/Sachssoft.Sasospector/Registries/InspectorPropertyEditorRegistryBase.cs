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

        public InspectorPropertyEditorRegistryBase()
        {
            _platformFactory = CreatePlatformFactory();
            //RegisterRules();
        }

        public InspectorPropertyEditorRegistryBase(IEnumerable<IInspectorPropertyEditorModule> modules)
        {
            _platformFactory = CreatePlatformFactory();

            foreach (var module in modules)
                AddModule(module);
        }

        public void Register(
            Type type,
            Func<IInspectorEditorPlatformFactory, IPropertyEditor> factory,
            int priority = 0)
        {
            var list = GetOrCreateList(type);
            list.Add(new Entry(factory, priority));
        }

        public void Register(
            Func<Type, bool> match,
            Func<IInspectorPropertyInfo, IInspectorEditorPlatformFactory, IPropertyEditor> factory,
            int priority = 0)
        {
            _rules.Add(new RuleEntry(match, factory, priority));
        }

        public void AddModule(IInspectorPropertyEditorModule module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            if (!_registeredModules.Add(module))
            {
                throw new InvalidOperationException(
                    $"Inspector module '{module.GetType().FullName}' is already registered.");
            }

            module.Register(this);
        }

        public IPropertyEditor? Create(IInspectorPropertyInfo info)
        {
            var type = info.Type;

            // 1. RULE SYSTEM (Semantik: Enum, Nullable, spezielle Fälle)
            var rule = _rules
                .Where(r => r.Match(type))
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault();

            if (rule != null)
                return rule.Factory(info, _platformFactory);

            // 2. REGISTRY SYSTEM (konkrete Typen)
            var candidates = _entries
                .Where(x => x.Key.IsAssignableFrom(type))
                .SelectMany(x => x.Value)
                .ToList();

            if (candidates.Count == 0)
                return null;

            return candidates
                .OrderByDescending(x => x.Priority)
                .First()
                .Factory(_platformFactory);
        }

        protected abstract IInspectorEditorPlatformFactory CreatePlatformFactory();

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