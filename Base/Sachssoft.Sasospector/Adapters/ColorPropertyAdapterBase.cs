using Sachssoft.Sasospector.Models;
using System;

namespace Sachssoft.Sasospector.Adapters
{
    public abstract class ColorPropertyAdapterBase : IInspectorPropertyAdapter
    {
        private protected ColorPropertyAdapterBase() { }

        private protected abstract Type SourceType { get; }

        protected abstract Color ToFieldOverride(object? sourceValue);

        protected abstract object? ToSourceOverride(Color adapterValue);

        public Color ToField(object? sourceValue)
        {
            ValidateType(sourceValue);

            return ToFieldOverride(sourceValue);
        }

        object? IInspectorPropertyAdapter.ToField(object? sourceValue)
        {
            return ToField(sourceValue);
        }

        public object? ToSource(object? adapterValue)
        {
            if (adapterValue is not Color color)
                throw new InvalidCastException(
                    $"Expected {typeof(Color).Name}, got {adapterValue?.GetType().Name ?? "null"}");

            return ToSourceOverride(color);
        }

        private void ValidateType(object? sourceValue)
        {
            var expected = SourceType;

            if (sourceValue is null)
                throw new InvalidCastException($"Expected {expected.Name}, got null");

            if (!expected.IsInstanceOfType(sourceValue))
                throw new InvalidCastException(
                    $"Expected {expected.Name}, got {sourceValue.GetType().Name}");
        }
    }
}