using Avalonia;
using Avalonia.Controls.Primitives;
using System;

namespace Sachssoft.Sasospector.Views
{
    public abstract class ConditionalViewItemBase : ContainerViewItem
    {
        private bool _isValueTypeMatched;

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

        protected override void OnUpdatePropertyEnter(IInspectorPropertyInfo property)
        {
            base.OnUpdatePropertyEnter(property);

            property.ValueChanged += OnPropertyValueChanged;
            UpdateMatch();
        }

        protected override void OnUpdatePropertyExit(IInspectorPropertyInfo property)
        {
            base.OnUpdatePropertyExit(property);

            property.ValueChanged -= OnPropertyValueChanged;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ActiveModelProperty ||
                change.Property == ActiveSchemaProperty)
            {
                UpdateMatch();
            }
        }

        private void OnPropertyValueChanged(object? sender, InspectorPropertyChangedEventArgs e)
        {
            UpdateMatch();
        }

        protected abstract bool Match(object? propertyValue, Type? propertyType);

        protected void UpdateMatch()
        {
            if (ActiveModel == null)
            {
                IsValueTypeMatched = false;
                return;
            }

            if (string.IsNullOrEmpty(PropertyName))
            {
                IsValueTypeMatched = false;
                return;
            }

            if (Property == null)
            {
                IsValueTypeMatched = false;
                return;
            }

            IsValueTypeMatched = Match(Property.GetValue(ActiveModel), Property.Type);
        }
    }
}