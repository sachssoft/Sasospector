using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class ToggleableSectionView : SectionView
    {
        private IInspectorPropertyInfo? _property;

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<PropertyViewItem, string?>(nameof(PropertyName));

        public static readonly DirectProperty<ToggleableSectionView, IInspectorPropertyInfo?> PropertyProperty =
            AvaloniaProperty.RegisterDirect<ToggleableSectionView, IInspectorPropertyInfo?>(
                nameof(Property),
                o => o.Property);

        protected override Type StyleKeyOverride { get; } = typeof(ToggleableSectionView);

        // Nur Boolean, Int z.b.0 oder 1, ...
        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        // Nur ReadOnly wichtig für Bindings
        public IInspectorPropertyInfo? Property
        {
            get => _property;
            private set => SetAndRaise(PropertyProperty, ref _property, value);
        }
    }
}
