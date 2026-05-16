using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Views.Fields
{
    public class ConditionalFieldHeader : FieldHeaderBase
    {
        public static readonly StyledProperty<object?> TargetValueProperty =
            AvaloniaProperty.Register<ConditionalFieldHeader, object?>(nameof(TargetValue));

        public static readonly StyledProperty<Type?> TargetTypeProperty =
            AvaloniaProperty.Register<ConditionalFieldHeader, Type?>(nameof(TargetType));

        public static readonly StyledProperty<ConditionalRule> RuleProperty =
            AvaloniaProperty.Register<ConditionalFieldHeader, ConditionalRule>(nameof(Rule));

        public static readonly StyledProperty<TypeEquation> TargetTypeEquationProperty =
            AvaloniaProperty.Register<ConditionalFieldHeader, TypeEquation>(nameof(TargetTypeEquation));

        // Soll-Value
        public object? TargetValue
        {
            get => GetValue(TargetValueProperty);
            set => SetValue(TargetValueProperty, value);
        }

        // Soll-Typ
        public Type? TargetType
        {
            get => GetValue(TargetTypeProperty);
            set => SetValue(TargetTypeProperty, value);
        }

        public ConditionalRule Rule
        {
            get => GetValue(RuleProperty);
            set => SetValue(RuleProperty, value);
        }

        public TypeEquation TargetTypeEquation
        {
            get => GetValue(TargetTypeEquationProperty);
            set => SetValue(TargetTypeEquationProperty, value);
        }

        public override bool Match(int index, Type? dataType, object? dataValue)
        {
            bool typeMatch = true;

            if (Rule != ConditionalRule.Value)
            {
                typeMatch = TargetTypeEquation switch
                {
                    TypeEquation.Exact =>
                        dataType != null && TargetType != null &&
                        dataType == TargetType,

                    TypeEquation.Assignable =>
                        dataType != null &&
                        TargetType != null &&
                        TargetType.IsAssignableFrom(dataType),

                    TypeEquation.BaseOnly =>
                        dataType?.BaseType == TargetType,

                    TypeEquation.Strict =>
                        dataType != null &&
                        TargetType != null &&
                        dataType == TargetType &&
                        Nullable.GetUnderlyingType(TargetType) == null,

                    TypeEquation.Compatible =>
                        dataType == TargetType ||
                        (TargetType?.IsAssignableFrom(dataType) ?? false),

                    _ => dataType == TargetType
                };
            }

            bool valueMatch = Rule switch
            {
                ConditionalRule.Type =>
                    typeMatch,

                ConditionalRule.Value =>
                    Equals(dataValue, TargetValue),

                _ =>
                    typeMatch && Equals(dataValue, TargetValue)
            };

            return valueMatch;
        }
    }
}
