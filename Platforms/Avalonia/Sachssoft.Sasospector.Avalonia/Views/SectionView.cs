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

        public static readonly StyledProperty<bool> IsExpandedProperty =
            AvaloniaProperty.Register<SectionView, bool>(nameof(IsExpanded));

        public static readonly StyledProperty<bool> IsExpandableProperty =
            AvaloniaProperty.Register<SectionView, bool>(nameof(IsExpandable));

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

        public bool IsExpanded
        {
            get => GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public bool IsExpandable
        {
            get => GetValue(IsExpandableProperty);
            set => SetValue(IsExpandableProperty, value);
        }

        public InspectorActions Actions { get; } = new InspectorActions();

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);


        }
    }
}
