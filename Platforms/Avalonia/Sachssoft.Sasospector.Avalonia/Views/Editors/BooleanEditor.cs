using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Sachssoft.Sasospector.Editors;
using System;
using System.Diagnostics;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class BooleanEditor : InspectorPropertyEditorBase, IBooleanEditor
    {
        private bool _kindSyncing;
        private bool _sourceSyncing;

        public static readonly StyledProperty<bool> ValueProperty =
            AvaloniaProperty.Register<BooleanEditor, bool>(nameof(Value));

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
                    Build();
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

        public bool Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == PreferredKindProperty && !_kindSyncing)
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

        protected override void OnPropertySourceChanged()
        {
            if (_sourceSyncing)
                return;

            _sourceSyncing = true;

            Value = (bool)Source!.GetValue()!;

            _sourceSyncing = false;
        }

        private void Build()
        {

        }
    }
}
