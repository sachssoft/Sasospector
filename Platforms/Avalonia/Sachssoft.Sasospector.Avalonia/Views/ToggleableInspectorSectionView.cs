using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class ToggleableInspectorSectionView : InspectorSectionView
    {
        private IInspectorPropertyInfo? _property;

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<InspectorPropertyView, string?>(nameof(PropertyName));

        public static readonly DirectProperty<ToggleableInspectorSectionView, IInspectorPropertyInfo?> PropertyProperty =
            AvaloniaProperty.RegisterDirect<ToggleableInspectorSectionView, IInspectorPropertyInfo?>(
                nameof(Property),
                o => o.Property);

        protected override Type StyleKeyOverride { get; } = typeof(ToggleableInspectorSectionView);

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
