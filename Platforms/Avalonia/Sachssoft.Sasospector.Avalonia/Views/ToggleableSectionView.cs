using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class ToggleableSectionView : SectionView
    {
        public static readonly StyledProperty<bool> IsCheckedProperty =
            AvaloniaProperty.Register<ToggleableSectionView, bool>(nameof(IsChecked));

        public static readonly StyledProperty<ToggleableSectionAction> ActionProperty =
            AvaloniaProperty.Register<ToggleableSectionView, ToggleableSectionAction>(nameof(Action));

        protected override Type StyleKeyOverride { get; } = typeof(ToggleableSectionView);

        public bool IsChecked
        {
            get => GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public ToggleableSectionAction Action
        {
            get => GetValue(ActionProperty);
            set => SetValue(ActionProperty, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == PropertyProperty)
            {
                Update();
            }
        }

        private void Update()
        {
            // später: Sync Logic zwischen Property + UI
        }
    }
}