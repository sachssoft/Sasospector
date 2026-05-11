using System.Collections.Generic;

namespace Sachssoft.Sasospector.Schemas
{
    public interface IInspectorSchema : IEnumerable<IInspectorPropertyInfo>
    {
        object Owner { get; }

        IReadOnlyDictionary<string, IInspectorPropertyInfo> Properties { get; }

        bool TryGet(string name, out IInspectorPropertyInfo? property);
    }
}
