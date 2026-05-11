using System;
using System.Text.RegularExpressions;

namespace Sachssoft.Sasospector.Constraints
{
    public class RegexConstraint : InspectorConstraintBase<string?>
    {
        public required string Pattern { get; init; }

        public RegexOptions Options { get; init; } = RegexOptions.None;

        public override ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success();

            if (!Regex.IsMatch(value, Pattern, Options))
            {
                return ValidationResult.Fail(
                    new ArgumentException($"Value does not match pattern '{Pattern}'"));
            }

            return ValidationResult.Success();
        }
    }
}