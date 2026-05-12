using Avalonia.Controls.Templates;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorSelectorView : InspectorItem
    {
        public InspectorSelectorView() { }

        public IDataTemplate DataTemplate
        {
            get; set;
        }
    }
}
