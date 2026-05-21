using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Sachssoft.Sasospector.Constraints;
using Sachssoft.Sasospector.Editors;
using Sachssoft.Sasospector.Views.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class InstanceSelector : PropertyEditorBase, IInstanceSelector
    {
        private ObjectEditorMode _editorMode = ObjectEditorMode.None;
        private bool _sourceSyncing;
        private IEnumerable<InstanceSelectorField> _fields = Array.Empty<InstanceSelectorField>();
        private bool _distinctTypesOnly;
        private bool _autoAddMissingInstances;
        private IInstanceSelectionConstraint? _constraint;
        public static readonly StyledProperty<bool> AllowNullSelectionProperty =
            AvaloniaProperty.Register<InstanceSelector, bool>(nameof(AllowNullSelection));

        public static readonly DirectProperty<InstanceSelector, IEnumerable<InstanceSelectorField>> FieldsProperty =
            AvaloniaProperty.RegisterDirect<InstanceSelector, IEnumerable<InstanceSelectorField>>(
                nameof(Fields),
                o => o.Fields);

        public static readonly StyledProperty<IReadOnlyList<object?>?> InstancesProperty =
            AvaloniaProperty.Register<InstanceSelector, IReadOnlyList<object?>?>(nameof(Instances));

        public static readonly StyledProperty<int> SelectedInstanceIndexProperty =
            AvaloniaProperty.Register<InstanceSelector, int>(
                nameof(SelectedInstanceIndex), defaultValue: -1);

        public static readonly StyledProperty<IDataTemplate?> InstanceFieldTemplateProperty =
            AvaloniaProperty.Register<InstanceSelector, IDataTemplate?>(nameof(InstanceFieldTemplate));

        public static readonly DirectProperty<InstanceSelector, ObjectEditorMode> EditorModeProperty =
            AvaloniaProperty.RegisterDirect<InstanceSelector, ObjectEditorMode>(
                nameof(EditorMode),
                o => o.EditorMode);

        public static readonly DirectProperty<InstanceSelector, bool> DistinctTypesOnlyProperty =
            AvaloniaProperty.RegisterDirect<InstanceSelector, bool>(
                nameof(DistinctTypesOnly),
                o => o.DistinctTypesOnly);

        public static readonly DirectProperty<InstanceSelector, bool> AutoAddMissingInstancesProperty =
            AvaloniaProperty.RegisterDirect<InstanceSelector, bool>(
                nameof(AutoAddMissingInstances),
                o => o.AutoAddMissingInstances);

        protected override Type StyleKeyOverride { get; } = typeof(InstanceSelector);

        public bool AllowNullSelection
        {
            get => GetValue(AllowNullSelectionProperty);
            set => SetValue(AllowNullSelectionProperty, value);
        }

        public IEnumerable<InstanceSelectorField> Fields
        {
            get => _fields;
            private set => SetAndRaise(FieldsProperty, ref _fields, value);
        }

        public IReadOnlyList<object?>? Instances
        {
            get => GetValue(InstancesProperty);
            set => SetValue(InstancesProperty, value);
        }

        public int SelectedInstanceIndex
        {
            get => GetValue(SelectedInstanceIndexProperty);
            set => SetValue(SelectedInstanceIndexProperty, value);
        }

        public IDataTemplate? InstanceFieldTemplate
        {
            get => GetValue(InstanceFieldTemplateProperty);
            set => SetValue(InstanceFieldTemplateProperty, value);
        }

        // Nur ReadOnly wichtig für Bindings
        public ObjectEditorMode EditorMode
        {
            get => _editorMode;
            private set => SetAndRaise(EditorModeProperty, ref _editorMode, value);
        }

        public bool DistinctTypesOnly
        {
            get => _distinctTypesOnly;
            private set => SetAndRaise(DistinctTypesOnlyProperty, ref _distinctTypesOnly, value);
        }

        public bool AutoAddMissingInstances
        {
            get => _autoAddMissingInstances;
            private set => SetAndRaise(AutoAddMissingInstancesProperty, ref _autoAddMissingInstances, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == AllowNullSelectionProperty ||
                change.Property == EditorModeProperty ||
                //change.Property == InstancesProperty ||
                change.Property == SourceProperty)
            {
                //Build();
            }
            else if (change.Property == SelectedInstanceIndexProperty && !_sourceSyncing)
            {
                _sourceSyncing = true;

                if (Instances != null && SelectedInstanceIndex >= 0 && SelectedInstanceIndex < Instances.Count)
                {
                    Source?.SetValue(Instances[SelectedInstanceIndex]);
                }

                _sourceSyncing = false;
            }
        }

        protected override void OnPropertySourceValueChanged()
        {
            base.OnPropertySourceValueChanged();

            _sourceSyncing = true;

            //EnsureSelection();

            _sourceSyncing = false;
        }

        protected override void OnContainerEnter()
        {
            base.OnContainerEnter();

            Build();
        }

        private void Build()
        {
            if (Source == null)
            {
                Fields = Array.Empty<InstanceSelectorField>();
                return;
            }

            var propertyValue = Source.GetValue();
            List<object> candidates = new List<object>();
            int selectedInstanceIndex = 0;
            bool distinctTypesOnly = true;
            bool autoAddMissingInstances = false;

            if (Source.IsReadOnly) // Expandiert
            {
                if (propertyValue == null)
                {
                    EditorMode = ObjectEditorMode.None;
                    Fields = Array.Empty<InstanceSelectorField>();
                    return;
                }

                EditorMode = ObjectEditorMode.Expandable;
            }
            else
            {
                _constraint = Source.Metadata.Constraints?.OfType<IInstanceSelectionConstraint>()
                                                             .FirstOrDefault();

                if (_constraint != null)
                {
                    if (_constraint.Instances != null)
                        candidates.AddRange(_constraint.Instances);

                    if (_constraint.AllowNull)
                    {
                        candidates.Insert(0, null);
                    }
                    distinctTypesOnly = _constraint.DistinctTypesOnly;
                    autoAddMissingInstances = _constraint.AutoAddMissingInstances;
                    selectedInstanceIndex = int.Clamp(_constraint.DefaultItemIndex, 0, candidates.Count - 1);
                }

                EditorMode = ObjectEditorMode.Selectable;
            }

            if (candidates.Count == 0)
            {
                Fields = Array.Empty<InstanceSelectorField>();
                SelectedInstanceIndex = -1;
                return;
            }

            var fields = new List<InstanceSelectorField>();
            var propertyType = Source.Type;
            bool valueMatched = false;

            for (int i = 0; i < candidates.Count; i++)
            {
                var candidate = candidates[i];
                FieldHeaderBase? fieldHeader = null;

                TryMatchFieldHeader(i, propertyType, candidate, out fieldHeader);

                fields.Add(new InstanceSelectorField
                {
                    FieldHeader = fieldHeader,
                    Instance = candidate
                });

                if (!valueMatched && propertyValue == candidate)
                    valueMatched = true;
            }

            if (!valueMatched)
            {
                if (distinctTypesOnly)
                {
                    var runtimeType = propertyValue.GetType();

                    for (int i = 0; i < candidates.Count; i++)
                    {
                        var candidate = candidates[i];
                        if (candidate.GetType() == runtimeType)
                        {
                            candidates[i] = propertyValue;
                            valueMatched = true;
                            break;
                        }
                    }
                }
            }

            if (!valueMatched)
            {
                if (autoAddMissingInstances && !distinctTypesOnly)
                {
                    candidates.Add(propertyValue);
                }
            }

            Instances = candidates.AsReadOnly();
            SelectedInstanceIndex = int.Clamp(selectedInstanceIndex, 0, candidates.Count - 1);
            Fields = fields.AsReadOnly();
        }
    }
}
