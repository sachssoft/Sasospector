using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace Sachssoft.Sasospector.Views
{
    [TemplatePart(PART_Items, typeof(ItemsControl))]
    public class CollectionViewItem : InspectorItemBase
    {
        private const string PART_Items = nameof(PART_Items);

        private readonly ObservableCollection<object?> _observableItems = new();
        private ItemsControl? _partItems;

        protected override Type StyleKeyOverride => typeof(CollectionViewItem);

        //public ObservableCollection<object?> ObservableItems
        //{
        //    get => _observableItems;
        //}

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partItems = e.NameScope.Get<ItemsControl>(PART_Items);

            if (_partItems != null)
            {
                _partItems.ItemsSource = _observableItems;
            }

            SynchronizeToObservable();
        }

        protected override void OnUpdatePropertyEnter(IInspectorPropertyInfo property)
        {
            base.OnUpdatePropertyEnter(property);
            SynchronizeToObservable();
        }

        protected override void OnUpdatePropertyExit(IInspectorPropertyInfo property)
        {
            base.OnUpdatePropertyExit(property);
        }

        public override void RefreshProperty()
        {
            base.RefreshProperty();
            SynchronizeToObservable();
        }

        private void SynchronizeToObservable()
        {
            _observableItems.Clear();

            if (ResolvedModel == null || Property == null)
                return;

            var enumerable = Property.GetValue(ResolvedModel) as IEnumerable;

            if (enumerable == null)
                return;

            foreach (var item in enumerable)
            {
                _observableItems.Add(item);
            }
        }
    }
}
