using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class DelegateSelector : PropertyEditorBase, IDelegateSelector
    {
        private const string PART_ExecuteButton = nameof(PART_ExecuteButton);

        private Button? _partExecuteButton;
        private ObjectEditorMode _editorMode = ObjectEditorMode.None;
        private IEnumerable? _fields;

        public static readonly DirectProperty<DelegateSelector, IEnumerable?> FieldsProperty =
            AvaloniaProperty.RegisterDirect<DelegateSelector, IEnumerable?>(
                nameof(Fields),
                o => o.Fields);

        public static readonly StyledProperty<IReadOnlyList<Delegate>?> DelegatesProperty =
            AvaloniaProperty.Register<DelegateSelector, IReadOnlyList<Delegate>?>(nameof(Delegates));

        public static readonly StyledProperty<int> SelectedDelegateIndexProperty =
            AvaloniaProperty.Register<DelegateSelector, int>(nameof(SelectedDelegateIndex));

        public static readonly StyledProperty<IDataTemplate?> DelegateFieldTemplateProperty =
            AvaloniaProperty.Register<DelegateSelector, IDataTemplate?>(nameof(DelegateFieldTemplate));

        public static readonly DirectProperty<DelegateSelector, ObjectEditorMode> EditorModeProperty =
            AvaloniaProperty.RegisterDirect<DelegateSelector, ObjectEditorMode>(
                nameof(EditorMode),
                o => o.EditorMode);

        public static readonly StyledProperty<ICommand?> CommandProperty =
            AvaloniaProperty.Register<ListEditor, ICommand?>(nameof(Command));

        public static readonly StyledProperty<object?> CommandParameterProperty =
            AvaloniaProperty.Register<ListEditor, object?>(nameof(CommandParameter));

        protected override Type StyleKeyOverride { get; } = typeof(DelegateSelector);

        public IEnumerable? Fields
        {
            get => _fields;
            private set => SetAndRaise(FieldsProperty, ref _fields, value);
        }

        public IReadOnlyList<Delegate>? Delegates
        {
            get => GetValue(DelegatesProperty);
            set => SetValue(DelegatesProperty, value);
        }

        public int SelectedDelegateIndex
        {
            get => GetValue(SelectedDelegateIndexProperty);
            set => SetValue(SelectedDelegateIndexProperty, value);
        }

        public IDataTemplate? DelegateFieldTemplate
        {
            get => GetValue(DelegateFieldTemplateProperty);
            set => SetValue(DelegateFieldTemplateProperty, value);
        }

        // Nur ReadOnly wichtig für Bindings
        public ObjectEditorMode EditorMode
        {
            get => _editorMode;
            private set => SetAndRaise(EditorModeProperty, ref _editorMode, value);
        }

        public ICommand? Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partExecuteButton != null)
                _partExecuteButton.Click -= _partExecuteButton_Click;

            _partExecuteButton = e.NameScope.Get<Button>(PART_ExecuteButton);

            if (_partExecuteButton != null)
                _partExecuteButton.Click += _partExecuteButton_Click;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == EditorModeProperty ||
                //change.Property == DelegatesProperty ||
                change.Property == CurrentPropertyProperty)
            {
                //Rebuild();
            }
            else if (change.Property == SelectedDelegateIndexProperty)
            {
                if (Delegates != null && SelectedDelegateIndex >= 0 && SelectedDelegateIndex < Delegates.Count)
                {
                    CurrentProperty?.SetValue(CurrentModel, Delegates[SelectedDelegateIndex]);
                }
            }
        }

        protected override void OnContainerEnter()
        {
            base.OnContainerEnter();
            Rebuild();
        }

        private void _partExecuteButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Command != null &&
                button.Command.CanExecute(button.CommandParameter))
            {
                button.Command.Execute(button.CommandParameter);
            }

            var field = Fields?
                .Cast<object>()
                .ElementAtOrDefault(SelectedDelegateIndex);

            SetSelectedFieldValue(field);

            e.Handled = true;
        }

        private void Rebuild()
        {
            var value = CurrentProperty?.GetValue(CurrentModel);

            if (value is IEnumerable enumerable)
            {
                var delegateList = enumerable.Cast<Delegate>().ToList();
                var fields = new List<object>();

                for (int i = 0; i < delegateList.Count; i++)
                {
                    var d = delegateList[i];
                    if (TryMatchFieldHeader(i, d.GetType(), d, out var fieldHeader))
                    {
                        fields.Add(new DelegateSelectorField
                        {
                            Delegate = d,
                            FieldHeader = fieldHeader
                        });
                    }
                    else
                    {
                        fields.Add(d);
                    }
                }

                Fields = fields;
                SelectedDelegateIndex = 0;
            }
        }
    }
}
