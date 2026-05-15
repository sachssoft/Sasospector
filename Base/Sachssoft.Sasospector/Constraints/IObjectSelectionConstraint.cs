using System.Collections.Generic;

namespace Sachssoft.Sasospector.Constraints
{
    public interface IObjectSelectionConstraint : IInspectorConstraint
    {
        bool AllowNull { get; set; }

        int DefaultItemIndex { get; set; }

        IReadOnlyList<object?>? Instances { get; }
    }
}
