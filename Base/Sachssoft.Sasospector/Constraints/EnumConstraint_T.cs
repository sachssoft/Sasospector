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

        public EnumSelectionMode SelectionMode { get; init; }

        public override ValidationResult Validate(TEnum value)
        {
            // Wenn keine Werte definiert sind, gilt alles als gültig
            if (Values.Count == 0)
                return ValidationResult.Success();

            // Einzelauswahl-Modus: exakt ein definierter Enum-Wert ist erlaubt
            if (SelectionMode != EnumSelectionMode.Multiple)
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

            // Mehrfachauswahl-Modus (Bitmask-Logik)
            ulong allowedBits = 0;

            // Alle erlaubten Flags zu einer Bitmaske zusammenfassen
            foreach (var v in Values)
                allowedBits |= Convert.ToUInt64(v.Value);

            // Aktuellen Wert als Bitmaske interpretieren
            ulong actual = Convert.ToUInt64(value);

            // Prüfen ob unerlaubte Bits gesetzt sind
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