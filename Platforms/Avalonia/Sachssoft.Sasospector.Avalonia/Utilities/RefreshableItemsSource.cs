using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Utilities
{
    public class RefreshableItemsSource :
        INotifyPropertyChanged,
        INotifyCollectionChanged
    {
        private IList _items = new List<object>();

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public IList Items => _items;

        public void Set(IList items)
        {
            if (_items is INotifyCollectionChanged oldNotify)
            {
                oldNotify.CollectionChanged -= Items_CollectionChanged;
            }

            _items = items ?? new List<object>();

            if (_items is INotifyCollectionChanged newNotify)
            {
                newNotify.CollectionChanged += Items_CollectionChanged;
            }

            OnPropertyChanged(nameof(Items));
            RaiseReset();
        }

        public void Refresh()
        {
            RaiseReset();
        }

        public void Add(object item)
        {
            _items.Add(item);
            RaiseReset();
        }

        public bool Remove(object item)
        {
            if (!_items.Contains(item))
                return false;

            _items.Remove(item);

            RaiseReset();
            return true;
        }

        public void Clear()
        {
            _items.Clear();
            RaiseReset();
        }

        private void Items_CollectionChanged(
            object? sender,
            NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private void RaiseReset()
        {
            OnPropertyChanged(nameof(Items));

            CollectionChanged?.Invoke(
                this,
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(name));
        }
    }
}