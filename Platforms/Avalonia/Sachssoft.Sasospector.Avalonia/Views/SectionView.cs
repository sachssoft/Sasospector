using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class SectionView : ContainerViewItem
    {
        public static readonly StyledProperty<string?> CategoryNameProperty =
            AvaloniaProperty.Register<SectionView, string?>(nameof(CategoryName));

        public static readonly StyledProperty<int?> DisplayOrderProperty =
            AvaloniaProperty.Register<SectionView, int?>(nameof(DisplayOrder));

        protected override Type StyleKeyOverride { get; } = typeof(SectionView);

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

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);


        }
    }
}
