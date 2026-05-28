using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class ListEditor : PropertyEditorBase, IListEditor, IItemTemplateProvider
    {
        private const string PART_List = nameof(PART_List);
        private const string PART_AddButton = nameof(PART_AddButton);
        private const string PART_RemoveButton = nameof(PART_RemoveButton);

        private ItemsControl? _partList;
        private Button? _partAddButton;
        private Button? _partRemoveButton;

        private ObservableCollection<object?> _observableItems = new();

        public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
            AvaloniaProperty.Register<ListEditor, IDataTemplate?>(nameof(ItemTemplate));

        public static readonly StyledProperty<ICommand?> AddCommandProperty =
            AvaloniaProperty.Register<ListEditor, ICommand?>(nameof(AddCommand));

        public static readonly StyledProperty<object?> AddCommandParameterProperty =
            AvaloniaProperty.Register<ListEditor, object?>(nameof(AddCommandParameter));

        public static readonly StyledProperty<ICommand?> RemoveCommandProperty =
            AvaloniaProperty.Register<ListEditor, ICommand?>(nameof(RemoveCommand));

        public static readonly StyledProperty<object?> RemoveCommandParameterProperty =
            AvaloniaProperty.Register<ListEditor, object?>(nameof(RemoveCommandParameter));

        public static readonly DirectProperty<ListEditor, ObservableCollection<object?>>
            ObservableItemsProperty =
                AvaloniaProperty.RegisterDirect<ListEditor, ObservableCollection<object?>>(
                    nameof(ObservableItems),
                    o => o.ObservableItems);

        protected override Type StyleKeyOverride { get; } = typeof(ListEditor);

        public ObservableCollection<object?> ObservableItems
        {
            get => _observableItems;
        }

        public ICommand? AddCommand
        {
            get => GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }

        public object? AddCommandParameter
        {
            get => GetValue(AddCommandParameterProperty);
            set => SetValue(AddCommandParameterProperty, value);
        }

        public ICommand? RemoveCommand
        {
            get => GetValue(RemoveCommandProperty);
            set => SetValue(RemoveCommandProperty, value);
        }

        public object? RemoveCommandParameter
        {
            get => GetValue(RemoveCommandParameterProperty);
            set => SetValue(RemoveCommandParameterProperty, value);
        }

        public IDataTemplate? ItemTemplate
        {
            get => GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            if (_partAddButton != null)
                _partAddButton.Click -= _partAddButton_Click;

            if (_partRemoveButton != null)
                _partRemoveButton.Click -= _partRemoveButton_Click;

            _partList = e.NameScope.Get<ItemsControl>(PART_List);
            _partAddButton = e.NameScope.Get<Button>(PART_AddButton);
            _partRemoveButton = e.NameScope.Get<Button>(PART_RemoveButton);

            if (_partAddButton != null)
                _partAddButton.Click += _partAddButton_Click;

            if (_partRemoveButton != null)
                _partRemoveButton.Click += _partRemoveButton_Click;
        }

        protected override void OnPropertySourceValueChanged()
        {
            base.OnPropertySourceValueChanged();
            SynchronizeToObservable();
        }

        protected override void OnContainerEnter()
        {
            base.OnContainerEnter();

            if (Container is PropertyViewItem propertyView)
            {
                foreach (var action in propertyView.Actions)
                {
                    if (action.Target == ListEditorActions.Add)
                    {
                        AddCommand = action.Command;
                        AddCommandParameter = action.CommandParameter;
                    }
                    else if (action.Target == ListEditorActions.Remove)
                    {
                        RemoveCommand = action.Command;
                        RemoveCommandParameter = action.CommandParameter;
                    }
                }
            }

            SynchronizeToObservable();
        }

        protected override void OnContainerExit()
        {
            base.OnContainerExit();

            AddCommand = null;
            RemoveCommand = null;
            AddCommandParameter = null;
            RemoveCommandParameter = null;
        }

        private void _partAddButton_Click(object? sender, RoutedEventArgs e)
        {
            OnButtonClick(sender as Button, e);
        }

        private void _partRemoveButton_Click(object? sender, RoutedEventArgs e)
        {
            OnButtonClick(sender as Button, e);
        }

        private void OnButtonClick(Button? button, RoutedEventArgs e)
        {
            if (button?.Command != null &&
                button.Command.CanExecute(button.CommandParameter))
            {
                button.Command.Execute(button.CommandParameter);
            }

            if (Container?.AutoSynchronization == true)
            {
                SynchronizeToObservable();
            }

            e.Handled = true;
        }

        private void SynchronizeToObservable()
        {
            var enumerable = CurrentProperty?.GetValue(CurrentModel) as IEnumerable;

            _observableItems.Clear();

            if (enumerable == null)
                return;

            foreach (var item in enumerable)
            {
                _observableItems.Add(item);
            }
        }
    }
}