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
    public class UriEditor : PropertyEditorBase, IUriEditor
    {
        private const string PART_TextBox = nameof(PART_TextBox);

        private TextBox? _partTextBox;
        private bool _sourceSyncing;

        protected override Type StyleKeyOverride { get; } = typeof(UriEditor);

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
            SyncFromSource();
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (_sourceSyncing || _partTextBox is null)
                return;

            if (e.Key == Key.Enter)
                SyncToSource();
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (_sourceSyncing || _partTextBox is null)
                return;

            SyncToSource();
        }

        private void SyncFromSource()
        {
            if (_partTextBox is null || CurrentProperty is null || CurrentModel is null)
                return;

            _sourceSyncing = true;
            try
            {
                var value = CurrentProperty.GetValue(CurrentModel);
                var text = value?.ToString();

                if (_partTextBox.Text != text)
                    _partTextBox.Text = text;
            }
            finally
            {
                _sourceSyncing = false;
            }
        }

        private void SyncToSource()
        {
            if (_partTextBox is null || CurrentProperty is null || CurrentModel is null)
                return;

            _sourceSyncing = true;
            try
            {
                var text = _partTextBox.Text;

                if (CurrentProperty.Type == typeof(string))
                {
                    CurrentProperty.SetValue(CurrentModel, text);
                    return;
                }

                if (CurrentProperty.Type == typeof(Uri))
                {
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        CurrentProperty.SetValue(CurrentModel, null);
                        return;
                    }

                    if (Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out var uri))
                    {
                        CurrentProperty.SetValue(CurrentModel, uri);
                    }

                    return;
                }
            }
            finally
            {
                _sourceSyncing = false;
            }
        }
    }
}