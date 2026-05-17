using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class ToggleableSectionView : SectionView
    {
        private IInspectorPropertyInfo? _property;

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<ToggleableSectionView, string?>(nameof(PropertyName));

        public static readonly DirectProperty<ToggleableSectionView, IInspectorPropertyInfo?> PropertyProperty =
            AvaloniaProperty.RegisterDirect<ToggleableSectionView, IInspectorPropertyInfo?>(
                nameof(Property),
                o => o.Property);

        public static readonly StyledProperty<bool> IsCheckedProperty =
            AvaloniaProperty.Register<ToggleableSectionView, bool>(nameof(IsChecked));

        public static readonly StyledProperty<ToggleableSectionMode> ModeProperty =
            AvaloniaProperty.Register<ToggleableSectionView, ToggleableSectionMode>(nameof(Mode));

        protected override Type StyleKeyOverride { get; } = typeof(ToggleableSectionView);

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public IInspectorPropertyInfo? Property
        {
            get => _property;
            private set => SetAndRaise(PropertyProperty, ref _property, value);
        }

        public bool IsChecked
        {
            get => GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public ToggleableSectionMode Mode
        {
            get => GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
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