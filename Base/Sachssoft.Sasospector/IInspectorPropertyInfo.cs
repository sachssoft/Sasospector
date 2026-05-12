using Sachssoft.Sasospector.Schemas;
using System;

namespace Sachssoft.Sasospector
{
    public interface IInspectorPropertyInfo
    {
        event EventHandler<InspectorPropertyChangingEventArgs>? Changing;
        event EventHandler<InspectorPropertyChangedEventArgs>? Changed;

        string Name { get; }

        Type Type { get; }

        IInspectorSchema Schema { get; }

        InspectorPropertyInfoMetadata Metadata { get; }

        bool IsReadOnly { get; }

        object? GetValue();

        void SetValue(object? value);
    }
}
