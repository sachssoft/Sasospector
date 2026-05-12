using Avalonia;

namespace Sachssoft.Sasospector.Views.Generators
{
    public class InspectorSectionGenerator : AvaloniaObject
    {
        public InspectorSectionView Build(object? categoryContext)
        {
            var view = new InspectorSectionView();
            return view;
        }
    }
}
