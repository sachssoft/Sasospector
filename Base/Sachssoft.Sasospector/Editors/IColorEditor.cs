using Sachssoft.Sasospector.Adapters;

namespace Sachssoft.Sasospector.Editors
{
    public interface IColorEditor : IPropertyEditor
    {
        InspectorPropertyAdapterBase<ColorValue>? Adapter { get; set; }

        bool IncludeAlpha { get; set; }
    }
}
