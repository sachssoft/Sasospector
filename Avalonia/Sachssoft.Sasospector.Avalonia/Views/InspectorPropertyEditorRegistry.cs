using Sachssoft.Sasospector.Views.Editors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sachssoft.Sasospector.Views
{
    public partial class InspectorPropertyEditorRegistry
    {
        private readonly Dictionary<Type, List<Entry>> _entries = new();
        private readonly List<RuleEntry> _rules = new();

        private sealed record Entry(
            Func<InspectorPropertyEditorBase> Factory,
            int Priority
        );

        private sealed record RuleEntry(
            Func<Type, bool> Match,
            Func<IInspectorPropertyInfo, InspectorPropertyEditorBase> Factory,
            int Priority
        );

        public static InspectorPropertyEditorRegistry Default { get; }
            = new InspectorPropertyEditorRegistry();

        public InspectorPropertyEditorRegistry()
        {
            RegisterPrimitiveTypes();
            RegisterPlatformDependencyTypes();
            RegisterCommonTypes();
            RegisterDataTimeTypes();
            RegisterNumericMathTypes();
            RegisterAvaloniaTypes();

            RegisterRules();
        }

        public void Register(
            Type type,
            Func<InspectorPropertyEditorBase> factory,
            int priority = 0)
        {
            var list = GetOrCreateList(type);
            list.Add(new Entry(factory, priority));
        }

        public void Register(
            Func<Type, bool> match,
            Func<IInspectorPropertyInfo, InspectorPropertyEditorBase> factory,
            int priority = 0)
        {
            _rules.Add(new RuleEntry(match, factory, priority));
        }

        public InspectorPropertyEditorBase? Create(IInspectorPropertyInfo info)
        {
            var type = info.Type;

            // 1. RULE SYSTEM (Semantik: Enum, Nullable, etc.)

            var rule = _rules
                .Where(r => r.Match(type))
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault();

            if (rule != null)
                return rule.Factory(info);

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
                .Factory();
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