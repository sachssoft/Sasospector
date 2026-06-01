using System;

namespace Sachssoft.Sasospector.Adapters
{
    public interface IInspectorPropertyAdapter
    {
        //bool SupportsField(Type fieldType);

        object? ToField(object? sourceValue);

        object? ToSource(object? fieldValue);
    }
}
