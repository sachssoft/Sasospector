using System;
using System.Numerics;

namespace Sachssoft.Sasospector.Constraints
{
    public class RangeConstraint<T> : InspectorConstraintBase<T>
        where T : struct, IMinMaxValue<T>, INumber<T>
    {
        public T MinValue { get; init; }
        public T MaxValue { get; init; }

        public override T Coerce(T value)
        {
            if (value < MinValue)
                return MinValue;

            if (value > MaxValue)
                return MaxValue;

            return value;
        }

        public override ValidationResult Validate(T value)
        {
            if (value < MinValue || value > MaxValue)
            {
                return ValidationResult.Fail(
                    new ArgumentOutOfRangeException(
                        $"Value {value} is outside range [{MinValue}, {MaxValue}]"));
            }

            return ValidationResult.Success();
        }
    }
}