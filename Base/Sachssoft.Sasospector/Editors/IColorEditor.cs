using Sachssoft.Sasospector.Adapters;

namespace Sachssoft.Sasospector.Editors
{
    public interface IColorEditor : IPropertyEditor
    {
        ColorPropertyAdapterBase Adapter { get; set; }

        bool IncludeAlpha { get; set; }
    }
}
