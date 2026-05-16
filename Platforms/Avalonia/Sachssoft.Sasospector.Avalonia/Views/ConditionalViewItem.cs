using Avalonia;
using Sachssoft.Sasospector.Registries;
using Sachssoft.Sasospector.Schemas;
using System;
using System.Diagnostics;

namespace Sachssoft.Sasospector.Views
{
    public class ConditionalViewItem : ContainerViewItem
    {
        private bool _isValueTypeMatched;
        private IInspectorPropertyInfo? _subscribedProperty;

        public static readonly StyledProperty<Type?> ValueTypeProperty =
            AvaloniaProperty.Register<ConditionalViewItem, Type?>(nameof(ValueType));

        public static readonly DirectProperty<ConditionalViewItem, bool> IsValueTypeMatchedProperty =
            AvaloniaProperty.RegisterDirect<ConditionalViewItem, bool>(
                nameof(IsValueTypeMatched),
                o => o.IsValueTypeMatched);

        public static readonly StyledProperty<ConditionalBehavior> BehaviorProperty =
            AvaloniaProperty.Register<ConditionalViewItem, ConditionalBehavior>(nameof(Behavior));

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<ConditionalViewItem, string?>(nameof(PropertyName));

        protected override Type StyleKeyOverride { get; } = typeof(ConditionalViewItem);

        public Type? ValueType
        {
            get => GetValue(ValueTypeProperty);
            set => SetValue(ValueTypeProperty, value);
        }

        public bool IsValueTypeMatched
        {
            get => _isValueTypeMatched;
            private set => SetAndRaise(IsValueTypeMatchedProperty, ref _isValueTypeMatched, value);
        }

        public ConditionalBehavior Behavior
        {
            get => GetValue(BehaviorProperty);
            set => SetValue(BehaviorProperty, value);
        }

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == SchemaProperty ||
                change.Property == PropertyNameProperty ||
                change.Property == ValueTypeProperty ||
                change.Property == BehaviorProperty)
            {
                UpdateSubscription();
                UpdateMatch();
            }
        }

        private void UpdateSubscription()
        {
            if (_subscribedProperty != null)
            {
                _subscribedProperty.ValueChanged -= OnPropertyChanged;
                _subscribedProperty = null;
            }

            if (Schema == null || string.IsNullOrEmpty(PropertyName))
                return;

            if (Schema.Properties.TryGetValue(PropertyName, out var propertyInfo))
            {
                _subscribedProperty = propertyInfo;
                _subscribedProperty.ValueChanged += OnPropertyChanged;
            }
        }

        private void OnPropertyChanged(object? sender, InspectorPropertyChangedEventArgs e)
        {
            UpdateMatch();
        }

        private void UpdateMatch()
        {
            if (Schema == null ||
                string.IsNullOrEmpty(PropertyName) ||
                ValueType == null ||
                !ValidateValueType())
            {
                IsValueTypeMatched = false;
                return;
            }

            if (!Schema.Properties.TryGetValue(PropertyName, out var propertyInfo))
            {
                IsValueTypeMatched = false;
                return;
            }

            var propertyValue = propertyInfo.GetValue();
            var propertyValueType = propertyValue?.GetType();

            IsValueTypeMatched = Behavior switch
            {
                ConditionalBehavior.Exact =>
                    ValueType == propertyValueType,

                ConditionalBehavior.Assignable =>
                    ValueType.IsAssignableFrom(propertyValueType),

                _ => false
            };

            Debug.WriteLine("IsValueTypeMatched {0}", IsValueTypeMatched);
        }

        private bool ValidateValueType()
        {
            if (ValueType != null && ValueType.IsClass)
                return true;
            return false;
        }
    }
}