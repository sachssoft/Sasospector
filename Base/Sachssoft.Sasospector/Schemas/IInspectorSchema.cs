using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Schemas
{
    public interface IInspectorSchema : IEnumerable<IInspectorPropertyInfo>
    {
        event EventHandler<InspectorSchemaSynchronizedEventArgs>? Synchronized;

        IReadOnlyDictionary<string, IInspectorPropertyInfo> Properties { get; }

        bool TryGet(string name, out IInspectorPropertyInfo? property);

        void RequestSynchronize(string propertyName);
    }
}
