using System.Collections.Generic;

namespace Sachssoft.Sasospector.Editors
{
    public interface IInstanceSelector : IPropertyEditor
    {
        bool AllowNullSelection { get; set; }

        IReadOnlyList<object?>? Instances { get; set; }

        int SelectedInstanceIndex { get; set; }
    }
}
