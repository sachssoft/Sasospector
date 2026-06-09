using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Editors;
using System;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class BooleanEditor : PropertyEditorBase, IBooleanEditor
    {
        private const string PART_Toggle = nameof(PART_Toggle);

        private bool _kindSyncing;
        private bool _sourceSyncing;
        private ToggleButton? _partToggle;

        public BooleanEditor()
        {
            EditorKindSelector = new EditorKindSelector(
                defaultKind: BooleanEditorKinds.Switch,
                changed: v =>
                {
                    if (_kindSyncing)
                        return;

                    _kindSyncing = true;
                    PreferredKind = v;
                    //Build();
                    _kindSyncing = false;
                },
                kinds: new[]
                {
                    BooleanEditorKinds.Switch,
                    BooleanEditorKinds.CheckBox,
                    BooleanEditorKinds.Toggle,
                    //BooleanEditorKinds.Dropdown,
                    //BooleanEditorKinds.Radio,
                    //BooleanEditorKinds.Segment
                }
            );
        }

        public EditorKindSelector EditorKindSelector { get; }

        protected override Type StyleKeyOverride { get; } = typeof(BooleanEditor);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partToggle != null)
            {
                _partToggle.IsCheckedChanged -= OnToggleCheckedChanged;
            }

            _partToggle = e.NameScope.Get<ToggleButton>(PART_Toggle);
            _partToggle.IsCheckedChanged += OnToggleCheckedChanged;

            SyncFromSource();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            if (_partToggle != null)
            {
                _partToggle.IsCheckedChanged -= OnToggleCheckedChanged;
            }
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == PreferredKindProperty && !_kindSyncing)
            {
                _kindSyncing = true;

                EditorKindSelector.Value = PreferredKind;
                //Build();

                _kindSyncing = false;
            }
        }

        protected override void OnPropertySourceValueChanged(InspectorPropertyChangedEventArgs e)
        {
            _sourceSyncing = true;
            SyncFromSource();
            _sourceSyncing = false;
        }

        private void OnToggleCheckedChanged(object? sender, RoutedEventArgs e)
        {
            _sourceSyncing = true;
            SyncToSource();
            _sourceSyncing = false;
        }

        private void SyncFromSource()
        {
            if (_partToggle is null || CurrentProperty is null || CurrentModel is null)
                return;

            if (CurrentProperty.Type == typeof(bool?))
            {
                _partToggle.IsChecked = (bool?)CurrentProperty.GetValue(CurrentModel);
            }
            else if (CurrentProperty.Type == typeof(bool))
            {
                _partToggle.IsChecked = (bool)CurrentProperty.GetValue(CurrentModel)!;
            }
        }

        private void SyncToSource()
        {
            if (_partToggle is null || CurrentProperty is null || CurrentModel is null)
                return;

            CurrentProperty.SetValue(CurrentModel, _partToggle.IsChecked);
        }
    }
}
