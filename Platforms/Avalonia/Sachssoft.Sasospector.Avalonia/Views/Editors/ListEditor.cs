using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections;
using System.Collections.ObjectModel;

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
        private InspectorAction? _addAction;
        private InspectorAction? _removeAction;
        private ObservableCollection<object?> _observableItems = new();

        public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
            AvaloniaProperty.Register<ListEditor, IDataTemplate?>(nameof(ItemTemplate));

        public static readonly DirectProperty<ListEditor, InspectorAction?> AddActionProperty =
            AvaloniaProperty.RegisterDirect<ListEditor, InspectorAction?>(
                nameof(AddAction),
                o => o.AddAction,
                (o, v) => o.AddAction = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

        public static readonly DirectProperty<ListEditor, InspectorAction?> RemoveActionProperty =
            AvaloniaProperty.RegisterDirect<ListEditor, InspectorAction?>(
                nameof(RemoveAction),
                o => o.RemoveAction,
                (o, v) => o.RemoveAction = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

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

        public InspectorAction? AddAction
        {
            get => _addAction;
            private set => SetAndRaise(AddActionProperty, ref _addAction, value);
        }

        public InspectorAction? RemoveAction
        {
            get => _removeAction;
            private set => SetAndRaise(RemoveActionProperty, ref _removeAction, value);
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

        protected override void OnPropertySourceValueChanged(InspectorPropertyChangedEventArgs e)
        {
            base.OnPropertySourceValueChanged(e);
            SynchronizeToObservable();
        }

        protected override void OnContainerEnter()
        {
            base.OnContainerEnter();

            foreach (var action in Actions)
            {
                if (action.Target == ListEditorActions.Add)
                {
                    AddAction = action;
                }
                else if (action.Target == ListEditorActions.Remove)
                {
                    RemoveAction = action;
                }
            }

            SynchronizeToObservable();
        }

        protected override void OnContainerExit()
        {
            base.OnContainerExit();

            AddAction = null;
            RemoveAction = null;
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

                var action = (InspectorAction)button.Tag!;
                action.RefreshItem?.RefreshProperty();
            }

            if (Container?.AutoSynchronization == true)
            {
                SynchronizeToObservable();
            }

            e.Handled = true;
        }

        private void SynchronizeToObservable()
        {
            _observableItems.Clear();

            if (CurrentProperty == null || CurrentModel == null)
                return;

            var enumerable = CurrentProperty?.GetValue(CurrentModel) as IEnumerable;

            if (enumerable == null)
                return;

            foreach (var item in enumerable)
            {
                _observableItems.Add(item);
            }
        }
    }
}