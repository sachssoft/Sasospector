using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class TypeConditionalViewItem : ConditionalViewItemBase
    {
        public static readonly StyledProperty<Type?> ValueTypeProperty =
            AvaloniaProperty.Register<ConditionalViewItemBase, Type?>(nameof(ValueType));

        public static readonly StyledProperty<ConditionalBehavior> BehaviorProperty =
            AvaloniaProperty.Register<TypeConditionalViewItem, ConditionalBehavior>(nameof(Behavior));

        public Type? ValueType
        {
            get => GetValue(ValueTypeProperty);
            set => SetValue(ValueTypeProperty, value);
        }

        public ConditionalBehavior Behavior
        {
            get => GetValue(BehaviorProperty);
            set => SetValue(BehaviorProperty, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ValueTypeProperty ||
                change.Property == BehaviorProperty)
            {
                UpdateMatch();
            }
        }

        protected override bool Match(object? propertyValue, Type? propertyType)
        {
            var runtimeType = propertyValue?.GetType();

            if (ValueType == null ||
            !ValidateValueType())
                return false;

            return Behavior switch
            {
                ConditionalBehavior.Exact =>
                    ValueType == runtimeType,

                ConditionalBehavior.Assignable =>
                    ValueType.IsAssignableFrom(runtimeType),

                _ => false
            };
        }

        private bool ValidateValueType()
        {
            if (ValueType != null && ValueType.IsClass)
                return true;
            return false;
        }
    }
}
