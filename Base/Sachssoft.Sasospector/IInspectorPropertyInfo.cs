using Sachssoft.Sasospector.Schemas;
using System;

namespace Sachssoft.Sasospector
{
    public interface IInspectorPropertyInfo
    {
        event EventHandler<InspectorPropertyChangingEventArgs>? ValueChanging;
        event EventHandler<InspectorPropertyChangedEventArgs>? ValueChanged;

        string Name { get; }

        Type Type { get; }

        IInspectorSchema Schema { get; }

        InspectorPropertyInfoMetadata Metadata { get; }

        bool IsReadOnly { get; }

        object? GetValue(object source);

        void SetValue(object source, object? value);
    }
}
