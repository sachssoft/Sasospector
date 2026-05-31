using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class OptionalPropertyViewItem : PropertyViewItem
    {

        public static readonly StyledProperty<bool> HasValueProperty =
            AvaloniaProperty.Register<OptionalPropertyViewItem, bool>(nameof(HasValue));

        protected override Type StyleKeyOverride => typeof(OptionalPropertyViewItem);

        public bool HasValue
        {
            get => GetValue(HasValueProperty);
            set => SetValue(HasValueProperty, value);
        }
    }
}
