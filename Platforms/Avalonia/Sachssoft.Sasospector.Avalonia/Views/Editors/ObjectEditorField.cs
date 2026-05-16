using Sachssoft.Sasospector.Views.Fields;

namespace Sachssoft.Sasospector.Views.Editors
{
    public record class ObjectEditorField
    {
        public FieldHeaderBase? FieldHeader { get; init; }

        public object? Instance { get; init; }
    }
}
