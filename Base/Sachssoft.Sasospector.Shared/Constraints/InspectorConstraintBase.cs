using System;

namespace Sachssoft.Sasospector.Constraints
{
    public abstract class InspectorConstraintBase<T> : IInspectorConstraint
    {
        public virtual bool CanHandle(IInspectorPropertyInfo property)
            => true;

        public virtual T Coerce(T baseValue)
            => baseValue;

        public virtual ValidationResult Validate(T? value)
            => ValidationResult.Success();

        object? IInspectorConstraint.Coerce(object? value)
        {
            if (value is not T typed)
                return value;

            return Coerce(typed);
        }

        ValidationResult IInspectorConstraint.Validate(object? value)
        {
            if (value is not T typed)
            {
                return ValidationResult.Fail(
                    new InvalidCastException($"Expected {typeof(T).Name}"));
            }

            return Validate(typed);
        }
    }
}
