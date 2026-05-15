using System.Collections.Generic;

namespace Sachssoft.Sasospector.Editors
{
    public interface IObjectEditor : IPropertyEditor
    {
        bool AllowNullSelection { get; set; }

        IReadOnlyList<object?>? Instances { get; set; }
    }
}
