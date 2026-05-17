namespace Sachssoft.Sasospector.Editors
{
    public interface IEnumEditor : IPropertyEditor
    {
        EditorKindSelector EditorKindSelector { get; }

        EnumSelectionMode SelectionMode { get; set; }
    }
}
