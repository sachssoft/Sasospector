using Avalonia;
using System;

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

        public override bool Match(int index, Type? propertyType, object? propertyValue)
        {
            return Rule switch
            {
                ConditionalRule.Type => MatchType(propertyType, propertyValue),
                ConditionalRule.Value => MatchValue(propertyValue),
                ConditionalRule.ValueAndType => MatchType(propertyType, propertyValue) &&
                                                MatchValue(propertyValue),
                _ => false
            };
        }

        private bool MatchType(Type? propertyType, object? propertyValue)
        {
            var runtimeType = propertyValue?.GetType();

            // Einen Treffer ist nur dann, wenn propertyValue exakt denselben Zieltyp hat

            bool typeMatch = TargetTypeEquation switch
            {
                TypeEquation.Exact =>
                    propertyType == TargetType,

                TypeEquation.Assignable =>
                    TargetType != null && TargetType.IsAssignableTo(propertyType),

                //TypeEquation.Strict =>
                //    runtimeType != null &&
                //    TargetType != null &&
                //    runtimeType == TargetType &&
                //    Nullable.GetUnderlyingType(runtimeType) == null,

                //TypeEquation.Compatible =>
                //    runtimeType == TargetType ||
                //    (runtimeType != null &&
                //     TargetType != null &&
                //     TargetType.IsAssignableFrom(runtimeType)),

                _ => false
            };

            // Einen Treffer ist nur dann, wenn propertyValue exakt denselben Zieltyp hat
            return typeMatch && (runtimeType == TargetType); //!!!!
        }

        private bool MatchValue(object? propertyValue)
        {
            return Equals(propertyValue, TargetValue);
        }
    }
}
