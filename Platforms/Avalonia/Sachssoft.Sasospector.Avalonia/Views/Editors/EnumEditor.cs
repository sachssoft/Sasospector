using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using Sachssoft.Sasospector.Editors;

namespace Sachssoft.Sasospector.Views.Editors
{
    [TemplatePart(PART_EnumFields, typeof(SelectingItemsControl))]
    public class EnumEditor : PropertyEditorBase, IEnumEditor
    {
        private const string PART_EnumFields = nameof(PART_EnumFields);

        private SelectingItemsControl? _partEnumFields;
        private bool _kindSyncing;
        private bool _sourceSyncing;
        private List<EnumItem>? _items;

        public static readonly StyledProperty<EnumSelectionMode> SelectionModeProperty =
            AvaloniaProperty.Register<EnumEditor, EnumSelectionMode>(nameof(SelectionMode));

        protected override Type StyleKeyOverride { get; } = typeof(EnumEditor);

        public EnumSelectionMode SelectionMode
        {
            get => GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        public EnumEditor()
        {
            SetupEditorKindSelector();
        }

        public EditorKindSelector EditorKindSelector { get; private set; }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partEnumFields != null)
                _partEnumFields.SelectionChanged -= OnSelectionChanged;

            _partEnumFields = e.NameScope.Get<SelectingItemsControl>(PART_EnumFields);

            if (_partEnumFields != null)
                _partEnumFields.SelectionChanged += OnSelectionChanged;

            Rebuild();
            SyncFromSource();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            if (_partEnumFields != null)
                _partEnumFields.SelectionChanged -= OnSelectionChanged;
        }

        protected override void OnPropertySourceValueChanged()
        {
            _sourceSyncing = true;
            SyncFromSource();
            _sourceSyncing = false;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == CurrentModelProperty ||
                change.Property == CurrentPropertyProperty)
            {
                Rebuild();
                SyncFromSource();
            }
            else if (change.Property == SelectionModeProperty)
            {
                SetupEditorKindSelector();
                Rebuild();
                SyncFromSource();
            }
            else if (change.Property == PreferredKindProperty && !_kindSyncing)
            {
                _kindSyncing = true;
                EditorKindSelector.Value = PreferredKind;
                Rebuild();
                SyncFromSource();
                _kindSyncing = false;
            }
        }

        private void Rebuild()
        {
            if (CurrentModel == null || CurrentProperty == null || _partEnumFields == null)
                return;

            if (!CurrentProperty.Type.IsEnum)
                return;

            var enumType = CurrentProperty.Type;
            var names = Enum.GetNames(enumType);

            _items = new List<EnumItem>();

            foreach (var name in names)
            {
                var value = Enum.Parse(enumType, name);
                _items.Add(new EnumItem(name, value));
            }

            _partEnumFields.ItemsSource = _items;
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (_sourceSyncing)
                return;

            _sourceSyncing = true;
            SyncToSource();
            _sourceSyncing = false;
        }

        private void SyncFromSource()
        {
            if (_partEnumFields == null || CurrentProperty == null || CurrentModel == null || _items == null)
                return;

            var value = CurrentProperty.GetValue(CurrentModel);

            _partEnumFields.SelectedItem = _items.FirstOrDefault(x => Equals(x.Value, value));
        }

        private void SyncToSource()
        {
            if (_partEnumFields == null || CurrentProperty == null || CurrentModel == null)
                return;

            if (_partEnumFields.SelectedItem is not EnumItem item)
                return;

            CurrentProperty.SetValue(CurrentModel, item.Value);
        }

        private void SetupEditorKindSelector()
        {
            var kinds = SelectionMode == EnumSelectionMode.Single
                ? new[]
                {
                    SingleSelectionEnumEditorKinds.Dropdown,
                    SingleSelectionEnumEditorKinds.Radio
                }
                : new[]
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
                    Rebuild();
                    SyncFromSource();
                    _kindSyncing = false;
                },
                kinds: kinds
            );
        }

        private class EnumItem
        {
            public string Name { get; }
            public object Value { get; }

            public EnumItem(string name, object value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString() => Name;
        }
    }
}