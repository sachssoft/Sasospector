using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Views.Editors
{
    [TemplatePart(PART_EnumFields, typeof(SelectingItemsControl))]
    public class EnumEditor : InspectorPropertyEditorBase, IEnumEditor
    {
        private const string PART_EnumFields = nameof(PART_EnumFields);

        private SelectingItemsControl? _partEnumFields;
        private string[]? _enumRawFields;
        private bool _kindSyncing;
        private bool _sourceSyncing;

        public static readonly StyledProperty<Enum> ValueProperty =
            AvaloniaProperty.Register<EnumEditor, Enum>(nameof(Value));

        public static readonly StyledProperty<EnumSelectionMode> SelectionModeProperty =
            AvaloniaProperty.Register<EnumEditor, EnumSelectionMode>(nameof(SelectionMode));

        public EnumEditor()
        {
            SetupEditorKindSelector();
        }

        public EditorKindSelector EditorKindSelector { get; private set; }

        protected override Type StyleKeyOverride { get; } = typeof(EnumEditor);

        public Enum Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public EnumSelectionMode SelectionMode
        {
            get => GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partEnumFields != null)
                _partEnumFields.SelectionChanged -= OnSelectionChanged;

            _partEnumFields = e.NameScope.Get<SelectingItemsControl>(PART_EnumFields);

            if (_partEnumFields != null)
                _partEnumFields.SelectionChanged += OnSelectionChanged;

            Build();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == SourceProperty)
            {
                Build();
            }
            else if (change.Property == SelectionModeProperty)
            {
                SetupEditorKindSelector();
                Build();
            }
            else if (change.Property == PreferredKindProperty && !_kindSyncing)
            {
                _kindSyncing = true;

                EditorKindSelector.Value = PreferredKind;
                Build();

                _kindSyncing = false;
            }
            else if (change.Property == ValueProperty && !_sourceSyncing)
            {
                _sourceSyncing = true;

                Source?.SetValue(Value);

                _sourceSyncing = false;
            }
        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {
            if (_partEnumFields != null)
                _partEnumFields.SelectionChanged -= OnSelectionChanged;

            base.OnUnloaded(e);
        }

        private void SetupEditorKindSelector()
        {
            var kinds = SelectionMode == EnumSelectionMode.Single ?
                new[]
                {
                    SingleSelectionEnumEditorKinds.Dropdown,
                    SingleSelectionEnumEditorKinds.Radio
                } :
                new[]
                {
                    MultipleSelectionEnumEditorKinds.Radio,
                    MultipleSelectionEnumEditorKinds.Segment
                };

            EditorKindSelector = new EditorKindSelector(
                defaultKind: BooleanEditorKinds.Switch,
                changed: v =>
                {
                    if (_kindSyncing)
                        return;

                    _kindSyncing = true;
                    PreferredKind = v;
                    Build();
                    _kindSyncing = false;
                },
                kinds: kinds
            );
        }

        private void Build()
        {
            if (Source == null || _partEnumFields == null)
                return;

            if (!Source.Type.IsEnum)
                return;

            var enumType = Source.Type;

            var currentValue = Source.GetValue();
            var enumValueName = currentValue != null
                ? Enum.GetName(enumType, currentValue)
                : null;

            _enumRawFields = Enum.GetNames(enumType);

            var fields = new List<object?>();

            for (int i = 0; i < _enumRawFields.Length; i++)
            {
                var field = _enumRawFields[i];
                var value = Enum.Parse(enumType, field);

                if (TryMatchFieldHeader(i, enumType, value, out var fieldHeader))
                {
                    fields.Add(fieldHeader);
                }
                else
                {
                    fields.Add(field);
                }
            }

            _partEnumFields.ItemsSource = fields;
            _partEnumFields.SelectedIndex = enumValueName != null
                ? Array.IndexOf(_enumRawFields, enumValueName)
                : -1;
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (_partEnumFields == null || _enumRawFields == null || Source == null)
                return;

            var index = _partEnumFields.SelectedIndex;
            if (index < 0 || index >= _enumRawFields.Length)
                return;

            var selectedName = _enumRawFields[index];

            var value = Enum.Parse(Source.Type, selectedName);
            Source.SetValue(value);
        }
    }
}