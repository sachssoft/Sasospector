using Sachssoft.Sasospector.Schemas;

namespace Sachssoft.Sasospector
{
    public class InspectorPropertyChangedEventArgs
    {
        public InspectorPropertyChangedEventArgs(IInspectorPropertyInfo property)
        {
            Property = property;
        }

        public IInspectorPropertyInfo Property { get; }

        public IInspectorSchema Schema => Property.Schema;
    }
}
