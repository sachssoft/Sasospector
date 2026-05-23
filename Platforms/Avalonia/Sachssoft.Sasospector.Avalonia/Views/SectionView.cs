using Avalonia;
using Avalonia.Collections;
using Avalonia.Metadata;
using Sachssoft.Sasospector.Schemas;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class SectionView : InspectorItemBase
    {
        private readonly AvaloniaList<InspectorItemBase> _items = new();

        public static readonly StyledProperty<string?> CategoryNameProperty =
            AvaloniaProperty.Register<SectionView, string?>(nameof(CategoryName));

        public static readonly StyledProperty<int?> DisplayOrderProperty =
            AvaloniaProperty.Register<SectionView, int?>(nameof(DisplayOrder));

        public static readonly StyledProperty<IInspectorSchemaSource?> ItemSchemaSourceProperty =
            AvaloniaProperty.Register<SectionView, IInspectorSchemaSource?>(nameof(ItemSchemaSource));

        public static readonly StyledProperty<object?> ItemModelProperty =
            AvaloniaProperty.Register<SectionView, object?>(nameof(ItemModel));

        protected override Type StyleKeyOverride { get; } = typeof(SectionView);

        [Content]
        public AvaloniaList<InspectorItemBase> Items => _items;

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

        public IInspectorSchemaSource? ItemSchemaSource
        {
            get => GetValue(ItemSchemaSourceProperty);
            set => SetValue(ItemSchemaSourceProperty, value);
        }

        public object? ItemModel
        {
            get => GetValue(ItemModelProperty);
            set => SetValue(ItemModelProperty, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);


        }
    }
}
