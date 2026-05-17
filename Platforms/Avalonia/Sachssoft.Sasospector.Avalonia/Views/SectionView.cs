using Avalonia;
using Avalonia.Collections;
using Avalonia.Metadata;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class SectionView : InspectorItem
    {
        private readonly AvaloniaList<InspectorItem> _items = new();

        public static readonly StyledProperty<string?> CategoryNameProperty =
            AvaloniaProperty.Register<SectionView, string?>(nameof(CategoryName));

        public static readonly StyledProperty<int?> DisplayOrderProperty =
            AvaloniaProperty.Register<SectionView, int?>(nameof(DisplayOrder));

        //public static readonly StyledProperty<IEnumerable<InspectorPropertyView>?> ItemsSourceProperty =
        //    AvaloniaProperty.Register<InspectorSectionView, IEnumerable<InspectorPropertyView>?>(nameof(ItemsSource));

        protected override Type StyleKeyOverride { get; } = typeof(SectionView);

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
