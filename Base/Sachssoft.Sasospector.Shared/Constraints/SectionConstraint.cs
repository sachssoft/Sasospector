namespace Sachssoft.Sasospector.Constraints
{
    // Ein Objekt Expander (UI Constraint / Descriptor)
    public class SectionConstraint : IInspectorConstraint
    {
        public bool CanHandle(IInspectorPropertyInfo property)
        {
            if (property.Type.IsValueType)
                return false;

            if (!property.IsReadOnly)
                return false;

            // Null verhindert Expansion
            // (bewusst UI-Entscheidung)
            if (property.GetValue() == null)
                return false;

            return true;
        }

        object? IInspectorConstraint.Coerce(object? baseValue)
            => baseValue;

        ValidationResult IInspectorConstraint.Validate(object? value)
            => ValidationResult.Success();
    }
}