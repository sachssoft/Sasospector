namespace Sachssoft.Sasospector
{
    public interface IPropertyEditor
    {
        IInspectorPropertyInfo Source { get; }

        string? PreferredKind { get; set; }
    }
}
