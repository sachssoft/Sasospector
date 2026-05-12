using Sachssoft.Sasospector.Adapters;
using System;

namespace Sachssoft.Sasospector.Editors
{
    public interface IDateTimeEditor : IPropertyEditor
    {
        InspectorPropertyAdapterBase<DateTime> Adapter { get; set; }

        DateTimeEditorParts Parts { get; set; }
    }
}
