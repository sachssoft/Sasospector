using Sachssoft.Sasospector.Views.Fields;
using System;
using System.Windows.Input;

namespace Sachssoft.Sasospector.Views.Editors
{
    public record class DelegateSelectorField
    {
        public FieldHeaderBase? FieldHeader { get; init; }

        public Delegate? Delegate { get; init; }
    }
}
