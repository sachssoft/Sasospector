using System;
using System.Collections.Generic;
using System.Linq;

namespace Sachssoft.Sasospector.Constraints
{
    public class EnumConstraint<TEnum> : InspectorConstraintBase<TEnum>
        where TEnum : struct, Enum
    {
        public IReadOnlyList<EnumField<TEnum>> Values { get; init; }
            = Array.Empty<EnumField<TEnum>>();

        public bool IsFlags => typeof(TEnum)
            .IsDefined(typeof(FlagsAttribute), false);

        public override ValidationResult Validate(TEnum value)
        {
            if (Values.Count == 0)
                return ValidationResult.Success();

            if (!IsFlags)
            {
                if (!Values.Any(v =>
                    EqualityComparer<TEnum>.Default.Equals(v.Value, value)))
                {
                    return ValidationResult.Fail(
                        new InvalidOperationException(
                            $"Value '{value}' is not allowed"));
                }

                return ValidationResult.Success();
            }

            // FLAGS MODE
            ulong allowedBits = 0;

            foreach (var v in Values)
                allowedBits |= Convert.ToUInt64(v.Value);

            ulong actual = Convert.ToUInt64(value);

            if ((actual & ~allowedBits) != 0)
            {
                return ValidationResult.Fail(
                    new InvalidOperationException(
                        $"Value '{value}' contains invalid flags"));
            }

            return ValidationResult.Success();
        }
    }
}