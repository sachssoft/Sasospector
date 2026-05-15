using System;
using System.Collections.Generic;
using System.Linq;

namespace Sachssoft.Sasospector.Constraints
{
    public class ObjectSelectionConstraint<T> : IObjectSelectionConstraint
    {
        private IInspectorPropertyInfo? _property;
        private IReadOnlyList<object?>? _cachedInstances;

        public IReadOnlyList<T?>? Instances { get; init; }

        public bool AllowNull { get; set; }

        public int DefaultItemIndex { get; set; } = -1;

        public bool CanHandle(IInspectorPropertyInfo property)
        {
            _property = property;
            return !property.Type.IsValueType && !property.Type.IsArray;
        }

        public object? Coerce(object? baseValue)
        {
            return baseValue;
        }

        IReadOnlyList<object?>? IObjectSelectionConstraint.Instances
        {
            get
            {
                if (_cachedInstances != null)
                    return _cachedInstances;

                if (Instances == null)
                    return null;

                _cachedInstances = Instances
                    .Select(x => (object?)x)
                    .ToArray();

                return _cachedInstances;
            }
        }

        public ValidationResult Validate(object? value)
        {
            // Wenn die Eigenschaft ReadOnly ist und der Wert null ist,
            // ist der Zustand ungültig, da der Expander-Inspector keine null-Instanz verarbeiten kann.
            if (_property != null && _property.IsReadOnly && value == null)
            {
                return ValidationResult.Fail(new InvalidOperationException(
                    "Cannot assign null to a read-only property."));
            }

            // Null ist nur erlaubt, wenn explizit erlaubt
            if (value is null && !AllowNull)
            {
                return ValidationResult.Fail(new InvalidOperationException(
                    "Null selection is not allowed."));
            }

            return ValidationResult.Success();
        }
    }
}