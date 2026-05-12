using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Collections.Generic;
using System.Globalization;

namespace Sachssoft.Sasospector.Views.Editors
{
    public abstract class InspectorPropertyEditorBase : TemplatedControl
    {
        private CultureInfo _effectiveCulture = CultureInfo.CurrentUICulture;

        public static readonly StyledProperty<CultureInfo?> CultureProperty =
            AvaloniaProperty.Register<ItemsControl, CultureInfo?>(nameof(Culture));

        // null = CurrentCultureUI
        public CultureInfo? Culture
        {
            get => GetValue(CultureProperty);
            set => SetValue(CultureProperty, value);
        }

        public CultureInfo EffectiveCulture => _effectiveCulture;

        public InspectorPropertyInfo Source { get; internal set; }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == CultureProperty) { 
                _effectiveCulture = Culture ?? CultureInfo.CurrentUICulture;
            }
        }
    }
}
