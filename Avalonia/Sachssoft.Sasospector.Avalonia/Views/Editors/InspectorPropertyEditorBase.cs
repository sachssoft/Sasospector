using Avalonia.Controls.Primitives;

namespace Sachssoft.Sasospector.Views.Editors
{
    public abstract class InspectorPropertyEditorBase : TemplatedControl
    {

        public InspectorPropertyInfo Source { get; internal set; }
    }
}
