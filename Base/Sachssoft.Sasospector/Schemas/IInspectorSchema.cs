using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Schemas
{
    public interface IInspectorSchema : IEnumerable<IInspectorPropertyInfo>, INotifyPropertyChanged, INotifyPropertyChanging
    {
        IReadOnlyDictionary<string, IInspectorPropertyInfo> Properties { get; }

        bool TryGet(string name, out IInspectorPropertyInfo? property);
    }
}
