using Sachssoft.Sasospector.Adapters;

namespace Sachssoft.Sasospector.Editors
{
    public interface IDateTimeEditor : IPropertyEditor
    {
        DateTimePropertyAdapter Adapter { get; set; }

        DateTimeEditorParts Parts { get; set; }
    }
}
