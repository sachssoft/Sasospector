using Sachssoft.Sasospector.Adapters;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Editors
{
    public interface IMultipleNumericEditor : IPropertyEditor
    {
        IIndexedPropertyAdapter? Adapter { get; set; }

        IReadOnlyList<IMultipleNumericEditorField>? Fields { get; set; }

        IMultipleNumericEditorField CreateField(Type valueType);
    }
}
