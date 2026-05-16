using Avalonia;

namespace Sachssoft.Sasospector.Views.Generators
{
    public class InspectorPropertyGenerator : AvaloniaObject
    {
        public PropertyViewItem Build(object? ownerSource, object? propertySource)
        {
            var view = new PropertyViewItem();
            return view;
        }
    }
}
