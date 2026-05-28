namespace Sachssoft.Sasospector
{
    public interface IPropertyEditor
    {
        IInspectorPropertyInfo CurrentProperty { get; }

        string? PreferredKind { get; set; }
    }
}
