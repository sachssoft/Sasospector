namespace Sachssoft.Sasospector.Constraints
{
    public interface IInspectorConstraint
    {
        bool CanHandle(IInspectorPropertyInfo property);

        object? Coerce(object? baseValue);

        ValidationResult Validate(object? value);
    }
}
