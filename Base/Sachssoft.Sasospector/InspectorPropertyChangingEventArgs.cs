using Sachssoft.Sasospector.Schemas;

namespace Sachssoft.Sasospector
{
    public class InspectorPropertyChangingEventArgs
    {
        public InspectorPropertyChangingEventArgs(IInspectorPropertyInfo property)
        {
            Property = property;
        }

        public bool Cancel { get; set; }

        public IInspectorPropertyInfo Property { get; }

        public IInspectorSchema Schema => Property.Schema;
    }
}
