using System.Collections.Generic;

namespace Sachssoft.Sasospector.Constraints
{
    public class ObjectSelectionConstraint<T> : InspectorConstraintBase<T>
    {

        public IReadOnlyList<T>? Items { get; init; } = null;

        public bool AllowNull { get; set; }

        public int DefaultItemIndex { get; set; } = -1;

    }
}
