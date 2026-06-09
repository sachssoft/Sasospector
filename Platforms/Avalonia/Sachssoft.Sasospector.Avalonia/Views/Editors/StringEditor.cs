using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Editors;
using System;

namespace Sachssoft.Sasospector.Views.Editors
{
    [TemplatePart(PART_TextBox, typeof(TextBox))]
    public class StringEditor : PropertyEditorBase, IStringEditor
    {
        private const string PART_TextBox = nameof(PART_TextBox);

        private bool _sourceSyncing;
        private TextBox? _partTextBox;

        protected override Type StyleKeyOverride { get; } = typeof(StringEditor);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partTextBox != null)
            {
                _partTextBox.KeyDown -= OnKeyDown;
                _partTextBox.LostFocus -= OnLostFocus;
            }

            _partTextBox = e.NameScope.Get<TextBox>(PART_TextBox);

            _partTextBox.KeyDown += OnKeyDown;
            _partTextBox.LostFocus += OnLostFocus;

            SyncFromSource();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            if (_partTextBox != null)
            {
                _partTextBox.KeyDown -= OnKeyDown;
                _partTextBox.LostFocus -= OnLostFocus;
            }
        }

        protected override void OnPropertySourceValueChanged(InspectorPropertyChangedEventArgs e)
        {
            _sourceSyncing = true;
            SyncFromSource();
            _sourceSyncing = false;
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (_sourceSyncing)
                return;

            if (e.Key == Key.Enter)
            {
                _sourceSyncing = true;
                SyncToSource();
                _sourceSyncing = false;
            }
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (_sourceSyncing)
                return;

            _sourceSyncing = true;
            SyncToSource();
            _sourceSyncing = false;
        }

        private void SyncFromSource()
        {
            if (_partTextBox is null || CurrentProperty is null || CurrentModel is null)
                return;

            var value = (string?)CurrentProperty.GetValue(CurrentModel);

            _partTextBox.Text = value;
        }

        private void SyncToSource()
        {
            if (_partTextBox is null || CurrentProperty is null || CurrentModel is null)
                return;

            CurrentProperty.SetValue(CurrentModel, _partTextBox.Text);
        }
    }
}