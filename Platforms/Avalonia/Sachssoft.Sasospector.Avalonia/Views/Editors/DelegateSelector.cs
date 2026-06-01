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

namespace Sachssoft.Sasospector.Views.Editors
{
    public class DelegateSelector : PropertyEditorBase, IDelegateSelector
    {
        private const string PART_ExecuteButton = nameof(PART_ExecuteButton);

        private Button? _partExecuteButton;
        private ObjectEditorMode _editorMode = ObjectEditorMode.None;
        private IEnumerable? _fields;
        private InspectorAction? _action;

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

        public static readonly DirectProperty<DelegateSelector, InspectorAction?> ActionProperty =
            AvaloniaProperty.RegisterDirect<DelegateSelector, InspectorAction?>(
                nameof(Action),
                o => o.Action,
                (o, v) => o.Action = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

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

        public InspectorAction? Action
        {
            get => _action;
            private set => SetAndRaise(ActionProperty, ref _action, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partExecuteButton != null)
                _partExecuteButton.Click -= OnPartExecuteButtonClick;

            _partExecuteButton = e.NameScope.Get<Button>(PART_ExecuteButton);

            if (_partExecuteButton != null)
                _partExecuteButton.Click += OnPartExecuteButtonClick;
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
                    if (CurrentModel != null && CurrentProperty != null)
                        CurrentProperty.SetValue(CurrentModel, Delegates[SelectedDelegateIndex]);
                }
            }
        }

        protected override void OnContainerEnter()
        {
            base.OnContainerEnter();

            foreach (var action in Actions)
            {
                if (action.Target == DelegateEditorActions.Execute)
                {
                    Action = action;
                }
            }

            Rebuild();
        }

        protected override void OnContainerExit()
        {
            base.OnContainerExit();

            Action = null;
        }

        private void OnPartExecuteButtonClick(object? sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            var field = Fields?
                .Cast<object>()
                .ElementAtOrDefault(SelectedDelegateIndex);

            ApplySelectedField(field);

            if (button.Command != null &&
                button.Command.CanExecute(button.CommandParameter))
            {
                button.Command.Execute(button.CommandParameter);

                var action = (InspectorAction)button.Tag!;
                action.RefreshItem?.RefreshProperty();
            }

            e.Handled = true;
        }

        private void Rebuild()
        {
            if (CurrentModel == null || CurrentProperty == null)
            {
                Fields = null;
                Delegates = null;
                SelectedDelegateIndex = -1;
                return;
            }

            var value = CurrentProperty.GetValue(CurrentModel);

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

                ApplySelectedField(fields[SelectedDelegateIndex]);
            }
        }

        private void ApplySelectedField(object? value)
        {
            if (value is DelegateSelectorField field)
            {
                SetSelectedFieldValue(field.Delegate);
            }
            else if (value is Delegate d)
            {
                SetSelectedFieldValue(d);
            }
        }
    }
}
