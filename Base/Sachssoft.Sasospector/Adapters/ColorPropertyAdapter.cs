using System;

namespace Sachssoft.Sasospector.Adapters
{
    public class ColorPropertyAdapter : IInspectorPropertyAdapter
    {
        public bool SupportsField(Type type)
        {
            throw new NotImplementedException();
        }

        object? IInspectorPropertyAdapter.ToSource(object? adapterValue)
        {
            throw new NotImplementedException();
        }

        object? IInspectorPropertyAdapter.ToField(object? sourceValue)
        {
            throw new NotImplementedException();
        }
    }
}
