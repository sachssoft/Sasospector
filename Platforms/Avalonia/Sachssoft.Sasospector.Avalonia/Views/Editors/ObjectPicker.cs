using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class ObjectPicker : PropertyEditorBase, IObjectPicker
    {
        private const string PART_BrowseButton = nameof(PART_BrowseButton);
        private const string PART_DisplayContent = nameof(PART_DisplayContent);

        private bool _sourceSyncing;
        private Button? _partBrowseButton;
        private ContentControl? _partDisplayContent;
        private InspectorAction? _action;

        public static readonly DirectProperty<ObjectPicker, InspectorAction?> ActionProperty =
            AvaloniaProperty.RegisterDirect<ObjectPicker, InspectorAction?>(
                nameof(Action),
                o => o.Action,
                (o, v) => o.Action = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

        protected override Type StyleKeyOverride { get; } = typeof(ObjectPicker);

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

            _partBrowseButton = e.NameScope.Get<Button>(PART_BrowseButton);
            _partDisplayContent = e.NameScope.Get<ContentControl>(PART_DisplayContent);

            if (_partBrowseButton != null)
            {
                _partBrowseButton.Click += OnPartButtonBrowseClick;
            }

            SyncFromSource();
        }

        protected override void OnContainerEnter()
        {
            base.OnContainerEnter();

            foreach (var action in Actions)
            {
                if (action.Target == ObjectPickerAction.Pick)
                {
                    Action = action;
                }
            }
        }

        protected override void OnPropertySourceValueChanged(InspectorPropertyChangedEventArgs e)
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

        private void SyncFromSource()
        {
            if (_partDisplayContent is null || CurrentProperty is null || CurrentModel is null)
                return;

            var value = CurrentProperty.GetValue(CurrentModel);

            if (DisplayOverrideTemplate != null && DisplayOverrideTemplate.Match(value))
            {
                var displayOverride = DisplayOverrideTemplate.Build(value);
                _partDisplayContent.Content = displayOverride;
            }
            else
            {
                _partDisplayContent.Content = value;
            }
        }

        private void SyncToSource()
        {
            if (_partDisplayContent is null || CurrentProperty is null || CurrentModel is null)
                return;

            //CurrentProperty.SetValue(CurrentModel, _partDisplayLabel.Text);
        }
    }
}
