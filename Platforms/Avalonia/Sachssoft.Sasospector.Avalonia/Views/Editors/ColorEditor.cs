using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Editors;
using System;

namespace Sachssoft.Sasospector.Views.Editors
{
    [TemplatePart(PART_PickButton, typeof(Button))]
    [TemplatePart(PART_CaptureButton, typeof(Button))]
    [TemplatePart(PART_TextBox, typeof(TextBox))]
    public class ColorEditor : PropertyEditorBase, IColorEditor
    {
        private const string PART_PickButton = nameof(PART_PickButton);
        private const string PART_CaptureButton = nameof(PART_CaptureButton);
        private const string PART_TextBox = nameof(PART_TextBox);

        private bool _sourceSyncing;
        private Button? _partPickButton;
        private Button? _partCaptureButton;
        private TextBox? _partTextBox;
        private InspectorAction? _pickAction;
        private InspectorAction? _captureAction;

        public static readonly StyledProperty<ColorPropertyAdapterBase?> AdapterProperty =
            AvaloniaProperty.Register<ColorEditor, ColorPropertyAdapterBase?>(nameof(Adapter));

        public static readonly StyledProperty<bool> IncludeAlphaProperty =
            AvaloniaProperty.Register<ColorEditor, bool>(nameof(IncludeAlpha));

        public static readonly StyledProperty<Color> SelectedColorProperty =
            AvaloniaProperty.Register<ColorEditor, Color>(nameof(SelectedColor));

        public static readonly DirectProperty<ColorEditor, InspectorAction?> PickActionProperty =
            AvaloniaProperty.RegisterDirect<ColorEditor, InspectorAction?>(
                nameof(PickAction),
                o => o.PickAction,
                (o, v) => o.PickAction = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

        public static readonly DirectProperty<ColorEditor, InspectorAction?> CaptureActionProperty =
            AvaloniaProperty.RegisterDirect<ColorEditor, InspectorAction?>(
                nameof(CaptureAction),
                o => o.CaptureAction,
                (o, v) => o.CaptureAction = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

        protected override Type StyleKeyOverride { get; } = typeof(ColorEditor);

        public ColorPropertyAdapterBase? Adapter
        {
            get => GetValue(AdapterProperty);
            set => SetValue(AdapterProperty, value);
        }

        public bool IncludeAlpha
        {
            get => GetValue(IncludeAlphaProperty);
            set => SetValue(IncludeAlphaProperty, value);
        }

        public Color SelectedColor
        {
            get => GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public InspectorAction? PickAction
        {
            get => _pickAction;
            private set => SetAndRaise(PickActionProperty, ref _pickAction, value);
        }

        public InspectorAction? CaptureAction
        {
            get => _captureAction;
            private set => SetAndRaise(CaptureActionProperty, ref _captureAction, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partPickButton != null)
            {
                _partPickButton.Click -= OnPartButtonClick;
            }

            if (_partCaptureButton != null)
            {
                _partCaptureButton.Click -= OnPartButtonClick;
            }

            if (_partTextBox != null)
            {
                _partTextBox.KeyDown -= OnPartTextBoxKeyDown;
                _partTextBox.LostFocus -= OnPartTextBoxLostFocus;
            }

            _partPickButton = e.NameScope.Get<Button>(PART_PickButton);
            _partCaptureButton = e.NameScope.Get<Button>(PART_CaptureButton);
            _partTextBox = e.NameScope.Get<TextBox>(PART_TextBox);

            if (_partPickButton != null)
            {
                _partPickButton.Click += OnPartButtonClick;
            }

            if (_partCaptureButton != null)
            {
                _partCaptureButton.Click += OnPartButtonClick;
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
                if (action.Target == ColorEditorActions.Pick)
                {
                    PickAction = action;
                }
                else if (action.Target == ColorEditorActions.Capture)
                {
                    CaptureAction = action;
                }
            }
        }

        protected override void OnPropertySourceValueChanged()
        {
            _sourceSyncing = true;
            SyncFromSource();
            _sourceSyncing = false;
        }

        private void OnPartButtonClick(object? sender, RoutedEventArgs e)
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
            if (_partTextBox is null || CurrentProperty is null || CurrentModel is null || Adapter is null)
                return;

            var fieldValue = Adapter.ToField(CurrentProperty.GetValue(CurrentModel));
            _partTextBox.Text = fieldValue.ToString();
        }

        private void SyncToSource()
        {
            if (_partTextBox is null || CurrentProperty is null || CurrentModel is null || Adapter is null)
                return;

            var color = Sasospector.Models.Color.TryParse(_partTextBox.Text, out var result) ?
                result : new Sasospector.Models.Color(0, 0, 0, 0);

            var sourceValue = Adapter.ToSource(color);
            CurrentProperty.SetValue(CurrentModel, sourceValue);
        }
    }
}