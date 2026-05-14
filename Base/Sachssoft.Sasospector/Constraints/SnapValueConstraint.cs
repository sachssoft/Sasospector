using System;
using System.Collections.Generic;
using System.Numerics;

namespace Sachssoft.Sasospector.Constraints
{
    public class SnapValueConstraint<T> : InspectorConstraintBase<T>
        where T : struct, INumber<T>
    {
        public IReadOnlySet<T>? Values { get; init; }
        private T[] _ordered = Array.Empty<T>();

        public SnapValueConstraint()
        {
        }

        public SnapValueConstraint(IReadOnlySet<T> values)
        {
            Values = values;
            _ordered = new List<T>(values).ToArray();
            Array.Sort(_ordered);
        }

        public override T Coerce(T value)
        {
            if (_ordered.Length == 0)
                return value;

            T closest = _ordered[0];
            T bestDiff = T.Abs(_ordered[0] - value);

            for (int i = 1; i < _ordered.Length; i++)
            {
                T diff = T.Abs(_ordered[i] - value);

                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    closest = _ordered[i];
                }
            }

            return closest;
        }

        public override ValidationResult Validate(T value)
        {
            if (Values is null || Values.Count == 0)
                return ValidationResult.Success();

            if (Values.Contains(value))
                return ValidationResult.Success();

            return ValidationResult.Fail(
                new ArgumentOutOfRangeException(
                    $"Value {value} is not in allowed set."));
        }
    }
}