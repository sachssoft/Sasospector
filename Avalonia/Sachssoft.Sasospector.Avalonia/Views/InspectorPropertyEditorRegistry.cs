using Sachssoft.Sasospector.Info;
using Sachssoft.Sasospector.Views.Editors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorPropertyEditorRegistry
    {
        private readonly Dictionary<Type, List<Entry>> _entries = new();

        public static InspectorPropertyEditorRegistry Default { get; }
            = new InspectorPropertyEditorRegistry();

        private sealed record Entry(
            Func<InspectorPropertyEditorBase> Factory,
            string[]? Kinds,
            int Priority
        );

        public InspectorPropertyEditorRegistry()
        {
            RegisterDefaultEditors();
        }

        public void Register(
            Type propertyType,
            Func<InspectorPropertyEditorBase> factory,
            string[]? kinds = null,
            int priority = 0)
        {
            var list = GetOrCreateList(propertyType);
            list.Add(new Entry(factory, kinds, priority));
        }

        public void Replace(
            Type propertyType,
            Type existingEditorType,
            Func<InspectorPropertyEditorBase> newFactory)
        {
            if (!_entries.TryGetValue(propertyType, out var list))
                throw new InvalidOperationException("Property type not registered.");

            var index = list.FindIndex(x =>
                x.Factory.Method.DeclaringType == existingEditorType);

            if (index < 0)
            {
                throw new InvalidOperationException(
                    $"Editor '{existingEditorType.Name}' not found for '{propertyType.Name}'.");
            }

            list[index] = list[index] with { Factory = newFactory };
        }

        public InspectorPropertyEditorBase? Create(
            IInspectorPropertyInfo propertyInfo,
            Type? variantEditorType = null)
        {
            var propertyType = propertyInfo.Type;

            // Meta-Kinds aus der Property-Definition (z. B. "Switch", "Text", etc.)
            var metaKinds = propertyInfo.Metadata?.EditorKinds;

            // Alle möglichen Editor-Kandidaten für diesen Property-Type sammeln
            var candidates = _entries
                .Where(x => x.Key.IsAssignableFrom(propertyType))
                .SelectMany(x => x.Value)
                .ToList();

            if (candidates.Count == 0)
                return null;

            // ------------------------------------------------------------
            // 1. HARTE OVERRIDE-LOGIK (Callsite gewinnt immer)
            // ------------------------------------------------------------
            // Wenn der Entwickler explizit einen Editor-Type angibt,
            // wird dieser zwingend verwendet (sofern registriert).
            if (variantEditorType != null)
            {
                var match = candidates.FirstOrDefault(x =>
                    x.Factory.Method.DeclaringType == variantEditorType);

                if (match == null)
                {
                    throw new InvalidOperationException(
                        $"Editor variant '{variantEditorType.Name}' " +
                        $"is not registered for '{propertyType.Name}'.");
                }

                return match.Factory();
            }

            // ------------------------------------------------------------
            // 2. META-BASED FILTER (EditorKinds aus Property-Metadaten)
            // ------------------------------------------------------------
            // Wenn die Property bestimmte Kinds definiert (z. B. "Switch"),
            // werden nur passende Editor berücksichtigt.
            if (metaKinds is { Count: > 0 })
            {
                var filtered = candidates
                    .Where(e =>
                        e.Kinds != null &&
                        e.Kinds.Intersect(metaKinds, StringComparer.OrdinalIgnoreCase).Any())
                    .ToList();

                // Wenn passende Kandidaten gefunden wurden,
                // gewinnt der beste davon.
                if (filtered.Count > 0)
                {
                    return filtered
                        .OrderByDescending(x => x.Priority)
                        .First()
                        .Factory();
                }
            }

            // ------------------------------------------------------------
            // 3. DEFAULT-SELECTION (Fallback über Priority)
            // ------------------------------------------------------------
            // Wenn weder Override noch Meta-Kinds greifen,
            // wird einfach der Editor mit der höchsten Priority genommen.
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

        private void RegisterDefaultEditors()
        {
            // bool default editor
            Register(typeof(bool),
                () => new BooleanSwitchPropertyEditor(),
                kinds: [InspectorPropertyEditorKinds.Switch],
                priority: 0);

            // bool alternative editor (higher priority)
            //Register(typeof(bool),
            //    () => new BooleanComboEditor(),
            //    priority: 10);

            // string editor
            Register(typeof(string),
                () => new TextPropertyEditor(),
                kinds: [InspectorPropertyEditorKinds.Text],
                priority: 0);
        }
    }
}