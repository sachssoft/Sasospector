using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public abstract class ConditionalViewItemBase : ContainerViewItem
    {
        private bool _isValueTypeMatched;
        private IInspectorPropertyInfo? _subscribedProperty;

        public static readonly DirectProperty<ConditionalViewItemBase, bool> IsValueTypeMatchedProperty =
            AvaloniaProperty.RegisterDirect<ConditionalViewItemBase, bool>(
                nameof(IsValueTypeMatched),
                o => o.IsValueTypeMatched);

        protected override Type StyleKeyOverride { get; } = typeof(ConditionalViewItemBase);

        public bool IsValueTypeMatched
        {
            get => _isValueTypeMatched;
            private set => SetAndRaise(IsValueTypeMatchedProperty, ref _isValueTypeMatched, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == SchemaProperty ||
                change.Property == PropertyNameProperty)
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

        protected abstract bool Match(object? propertyValue, Type? propertyType);

        protected void UpdateMatch()
        {
            if (Schema == null ||
                string.IsNullOrEmpty(PropertyName))
            {
                IsValueTypeMatched = false;
                return;
            }

            if (!Schema.Properties.TryGetValue(PropertyName, out var propertyInfo))
            {
                IsValueTypeMatched = false;
                return;
            }

            IsValueTypeMatched = Match(propertyInfo.GetValue(), propertyInfo.Type);
        }
    }
}