using Sachssoft.Sasospector.Adapters;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Editors
{
    public interface IMultipleNumericEditor : IPropertyEditor
    {
        InspectorPropertyAdapterBase<BoundedValue<double>[]>? Adapter { get; set; }

        int? DecimalPlaces { get; set; }

        IReadOnlyList<EditorField>? Fields { get; set; }
    }
}
