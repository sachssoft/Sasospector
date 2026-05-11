using Avalonia;

namespace Sachssoft.Sasospector.Views.Generators
{
    public class InspectorPropertyGenerator : AvaloniaObject
    {
        public InspectorPropertyView Build(object? ownerSource, object? propertySource)
        {
            var view = new InspectorPropertyView();
            return view;
        }
    }
}
