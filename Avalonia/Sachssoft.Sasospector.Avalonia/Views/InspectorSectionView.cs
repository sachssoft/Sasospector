using Avalonia;
using Avalonia.Collections;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorSectionView : InspectorItem
    {
        private readonly AvaloniaList<InspectorItem> _items = new();

        public static readonly StyledProperty<string?> CategoryNameProperty =
            AvaloniaProperty.Register<InspectorSectionView, string?>(nameof(CategoryName));

        public static readonly StyledProperty<int?> DisplayOrderProperty =
            AvaloniaProperty.Register<InspectorSectionView, int?>(nameof(DisplayOrder));

        //public static readonly StyledProperty<IEnumerable<InspectorPropertyView>?> ItemsSourceProperty =
        //    AvaloniaProperty.Register<InspectorSectionView, IEnumerable<InspectorPropertyView>?>(nameof(ItemsSource));

        protected override Type StyleKeyOverride { get; } = typeof(InspectorSectionView); 
        
        [Content]
        public AvaloniaList<InspectorItem> Items => _items;

        public string? CategoryName
        {
            get => GetValue(CategoryNameProperty);
            set => SetValue(CategoryNameProperty, value);
        }

        public int? DisplayOrder
        {
            get => GetValue(DisplayOrderProperty);
            set => SetValue(DisplayOrderProperty, value);
        }

        //public IEnumerable<InspectorItem>? ItemsSource
        //{
        //    get => GetValue(ItemsSourceProperty);
        //    set => SetValue(ItemsSourceProperty, value);
        //}

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);


        }
    }
}
