namespace Sachssoft.Sasospector
{
    public interface IPropertyEditor
    {
        IInspectorPropertyInfo Source { get; }

        string? VariantKind { get; set; }
    }
}
