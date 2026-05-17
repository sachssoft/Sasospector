using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class BooleanConditionalViewItem : ConditionalViewItemBase
    {
        public static readonly StyledProperty<bool?> ExpectedValueProperty =
            AvaloniaProperty.Register<BooleanConditionalViewItem, bool?>(nameof(ExpectedValue));

        public bool? ExpectedValue
        {
            get => GetValue(ExpectedValueProperty);
            set => SetValue(ExpectedValueProperty, value);
        }

        protected override bool Match(object? propertyValue, Type? propertyType)
        {
            var current = propertyValue as bool?;

            if (propertyType == typeof(bool))
                return ExpectedValue == (bool?)propertyValue;

            if (propertyType == typeof(bool?))
                return ExpectedValue == current;

            return false;
        }
    }
}
