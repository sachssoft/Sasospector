using System;

namespace Sachssoft.Sasospector.Adapters
{
    public interface IIndexedPropertyAdapter : IInspectorPropertyAdapter
    {
        int FieldCount { get; }

        Type GetValueType(int fieldIndex);

        object? GetValue(int fieldIndex);

        void SetValue(int fieldIndex, object? value);
    }
}
