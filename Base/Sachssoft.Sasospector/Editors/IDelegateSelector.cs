using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Editors
{
    public interface IDelegateSelector : IPropertyEditor
    {
        IReadOnlyList<Delegate>? Delegates { get; set; }

        int SelectedDelegateIndex { get; set; }
    }
}