using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Sachssoft.Sasospector.Constraints;
using Sachssoft.Sasospector.Editors;
using Sachssoft.Sasospector.Views.Fields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class ObjectEditor : InspectorPropertyEditorBase, IObjectEditor
    {
        private ObjectEditorMode _editorMode = ObjectEditorMode.None;
        private bool _sourceSyncing;
        private IEnumerable<ObjectEditorField> _fields = Array.Empty<ObjectEditorField>();

        public static readonly StyledProperty<bool> AllowNullSelectionProperty =
            AvaloniaProperty.Register<ObjectEditor, bool>(nameof(AllowNullSelection));

        public static readonly DirectProperty<ObjectEditor, IEnumerable<ObjectEditorField>> FieldsProperty =
            AvaloniaProperty.RegisterDirect<ObjectEditor, IEnumerable<ObjectEditorField>>(
                nameof(Fields),
                o => o.Fields);

        public static readonly StyledProperty<IReadOnlyList<object?>?> InstancesProperty =
            AvaloniaProperty.Register<ObjectEditor, IReadOnlyList<object?>?>(nameof(Instances));

        public static readonly StyledProperty<int> SelectedInstanceIndexProperty =
            AvaloniaProperty.Register<ObjectEditor, int>(nameof(SelectedInstanceIndex));

        public static readonly StyledProperty<IDataTemplate?> InstanceFieldTemplateProperty =
            AvaloniaProperty.Register<ObjectEditor, IDataTemplate?>(nameof(InstanceFieldTemplate));

        public static readonly DirectProperty<ObjectEditor, ObjectEditorMode> EditorModeProperty =
            AvaloniaProperty.RegisterDirect<ObjectEditor, ObjectEditorMode>(
                nameof(EditorMode),
                o => o.EditorMode);

        protected override Type StyleKeyOverride { get; } = typeof(ObjectEditor);

        public bool AllowNullSelection
        {
            get => GetValue(AllowNullSelectionProperty);
            set => SetValue(AllowNullSelectionProperty, value);
        }

        public IEnumerable<ObjectEditorField> Fields
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

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            Build();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == AllowNullSelectionProperty ||
                change.Property == EditorModeProperty ||
                //change.Property == InstancesProperty ||
                change.Property == SourceProperty)
            {
                Build();
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

            if (Source != null &&
                Instances != null &&
                !_sourceSyncing)
            {
                var value = Source.GetValue();
                for (int i = 0; i < Instances.Count; i++)
                {
                    if (Instances[i] == value)
                    {
                        SelectedInstanceIndex = i;
                        break;
                    }
                }
            }

            _sourceSyncing = false;
        }

        private void Build()
        {
            if (Source == null)
            {
                Fields = Array.Empty<ObjectEditorField>();
                return;
            }

            var propertyValue = Source.GetValue();

            if (Source.IsReadOnly) // Expandiert
            {
                if (propertyValue == null)
                {
                    EditorMode = ObjectEditorMode.None;
                    Fields = Array.Empty<ObjectEditorField>();
                    return;
                }

                EditorMode = ObjectEditorMode.Expandable;
            }
            else
            {
                var constraint = Source.Metadata.Constraints?.OfType<IObjectSelectionConstraint>()
                                                             .FirstOrDefault();

                if (constraint != null)
                {
                    var instances = new List<object?>(constraint.Instances ?? Array.Empty<object?>());
                    if (constraint.AllowNull)
                    {
                        instances.Insert(0, null);
                    }
                    Instances = instances;
                    SelectedInstanceIndex = int.Clamp(constraint.DefaultItemIndex, 0, instances.Count - 1);
                }

                EditorMode = ObjectEditorMode.Selectable;
            }

            if (Instances == null)
            {
                Fields = Array.Empty<ObjectEditorField>();
                return;
            }

            var fields = new List<ObjectEditorField>();
            var propertyType = Source.Type;

            for(int i = 0; i < Instances.Count; i++)
            {
                var instance = Instances[i];
                FieldHeaderBase? fieldHeader = null;

                TryMatchFieldHeader(i, propertyType, instance, out fieldHeader);

                fields.Add(new ObjectEditorField
                {
                    FieldHeader = fieldHeader,
                    Instance = instance
                });
            }

            Fields = fields.AsReadOnly();
        }
    }
}
