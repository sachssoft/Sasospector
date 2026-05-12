using System;

namespace Sachssoft.Sasospector.Constraints
{
    public class StringConstraint : InspectorConstraintBase<string?>
    {
        public int? MinLength { get; init; }
        public int? MaxLength { get; init; }

        public bool AllowNullOrWhiteSpace { get; init; } = true;

        public override string? Coerce(string? value)
        {
            if (value == null)
                return value;

            if (MaxLength.HasValue && value.Length > MaxLength.Value)
                return value[..MaxLength.Value];

            return value;
        }

        public override ValidationResult Validate(string? value)
        {
            if (value is null)
            {
                return AllowNullOrWhiteSpace
                    ? ValidationResult.Success()
                    : ValidationResult.Fail(
                        new ArgumentException("Value cannot be null"));
            }

            if (!AllowNullOrWhiteSpace && string.IsNullOrWhiteSpace(value))
            {
                return ValidationResult.Fail(
                    new ArgumentException("Value cannot be whitespace"));
            }

            if (MinLength.HasValue && value.Length < MinLength.Value)
            {
                return ValidationResult.Fail(
                    new ArgumentOutOfRangeException(
                        $"String too short (min {MinLength.Value})"));
            }

            if (MaxLength.HasValue && value.Length > MaxLength.Value)
            {
                return ValidationResult.Fail(
                    new ArgumentOutOfRangeException(
                        $"String too long (max {MaxLength.Value})"));
            }

            return ValidationResult.Success();
        }
    }
}
