using Sachssoft.Sasospector.Models;
using System;

namespace Sachssoft.Sasospector.Adapters
{
    public sealed class ColorPropertyAdapter<T> : ColorPropertyAdapterBase
    {
        private readonly Func<T?, Color> _toField;
        private readonly Func<Color, T?> _toSource;

        public ColorPropertyAdapter(
            Func<T?, Color> toField,
            Func<Color, T?> toSource)
        {
            _toField = toField ?? throw new ArgumentNullException(nameof(toField));
            _toSource = toSource ?? throw new ArgumentNullException(nameof(toSource));
        }

        private protected override Type SourceType => typeof(T);

        protected override Color ToFieldOverride(object? sourceValue)
        {
            if (sourceValue is null)
                return default;

            if (sourceValue is not T typed)
                throw new InvalidCastException(
                    $"ColorAdapter: Expected {typeof(T).Name}, got {sourceValue.GetType().Name}");

            return _toField(typed);
        }

        protected override object? ToSourceOverride(Color adapterValue)
        {
            return _toSource(adapterValue);
        }
    }
}