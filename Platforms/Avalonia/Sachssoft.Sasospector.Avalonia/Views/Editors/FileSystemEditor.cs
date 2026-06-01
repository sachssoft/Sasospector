using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Views.Editors
{
    [TemplatePart(PART_BrowseButton, typeof(Button))]
    [TemplatePart(PART_TextBox, typeof(TextBox))]
    public class FileSystemEditor : PropertyEditorBase, IFileSystemEditor
    {
        private const string PART_BrowseButton = nameof(PART_BrowseButton);
        private const string PART_TextBox = nameof(PART_TextBox);

        private bool _sourceSyncing;
        private Button? _partBrowseButton;
        private TextBox? _partTextBox;
        private InspectorAction? _action;

        public static readonly StyledProperty<FileSystemMode> ModeProperty =
            AvaloniaProperty.Register<FileSystemEditor, FileSystemMode>(nameof(Mode));

        public static readonly StyledProperty<IEnumerable<string>?> FiltersProperty =
            AvaloniaProperty.Register<FileSystemEditor, IEnumerable<string>?>(nameof(Filters));

        public static readonly DirectProperty<FileSystemEditor, InspectorAction?> ActionProperty =
            AvaloniaProperty.RegisterDirect<FileSystemEditor, InspectorAction?>(
                nameof(Action),
                o => o.Action,
                (o, v) => o.Action = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

        protected override Type StyleKeyOverride { get; } = typeof(FileSystemEditor);

        public FileSystemMode Mode
        {
            get => GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public IEnumerable<string>? Filters
        {
            get => GetValue(FiltersProperty);
            set => SetValue(FiltersProperty, value);
        }

        public InspectorAction? Action
        {
            get => _action;
            private set => SetAndRaise(ActionProperty, ref _action, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partBrowseButton != null)
            {
                _partBrowseButton.Click -= OnPartButtonBrowseClick;
            }

            if (_partTextBox != null)
            {
                _partTextBox.KeyDown -= OnPartTextBoxKeyDown;
                _partTextBox.LostFocus -= OnPartTextBoxLostFocus;
            }

            _partBrowseButton = e.NameScope.Get<Button>(PART_BrowseButton);
            _partTextBox = e.NameScope.Get<TextBox>(PART_TextBox);

            if (_partBrowseButton != null)
            {
                _partBrowseButton.Click += OnPartButtonBrowseClick;
            }

            if (_partTextBox != null)
            {
                _partTextBox.KeyDown += OnPartTextBoxKeyDown;
                _partTextBox.LostFocus += OnPartTextBoxLostFocus;
            }

            SyncFromSource();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            if (_partTextBox != null)
            {
                _partTextBox.KeyDown -= OnPartTextBoxKeyDown;
                _partTextBox.LostFocus -= OnPartTextBoxLostFocus;
            }
        }

        protected override void OnContainerEnter()
        {
            base.OnContainerEnter();

            foreach (var action in Actions)
            {
                if (action.Target == FileSystemEditorActions.Browse)
                {
                    Action = action;
                }
            }
        }

        protected override void OnPropertySourceValueChanged()
        {
            _sourceSyncing = true;
            SyncFromSource();
            _sourceSyncing = false;
        }

        private void OnPartButtonBrowseClick(object? sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Command != null &&
                button.Command.CanExecute(button.CommandParameter))
            {
                button.Command.Execute(button.CommandParameter);

                var action = (InspectorAction)button.Tag!;
                action.RefreshItem?.RefreshProperty();
            }

            e.Handled = true;
        }

        private void OnPartTextBoxKeyDown(object? sender, KeyEventArgs e)
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

        private void OnPartTextBoxLostFocus(object? sender, RoutedEventArgs e)
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