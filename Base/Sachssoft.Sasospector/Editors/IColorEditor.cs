using Sachssoft.Sasospector.Adapters;

namespace Sachssoft.Sasospector.Editors
{
    public interface IColorEditor : IPropertyEditor
    {
        ColorPropertyAdapter? Adapter { get; set; }

        bool IncludeAlpha { get; set; }
    }
}
