using Avalonia.Collections;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorContainerTemplates : AvaloniaList<InspectorContainerTemplate>
    {
        public InspectorContainerTemplates()
        {
            ResetBehavior = ResetBehavior.Remove;
        }
    }
}
