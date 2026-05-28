using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Schemas
{
    public sealed class EmptyInspectorSchema : IInspectorSchema
    {
        private static readonly IReadOnlyDictionary<string, IInspectorPropertyInfo> EmptyProperties
            = new Dictionary<string, IInspectorPropertyInfo>();

        public static readonly EmptyInspectorSchema Instance = new();

        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add { }
            remove { }
        }

        event PropertyChangingEventHandler? INotifyPropertyChanging.PropertyChanging
        {
            add { }
            remove { }
        }

        IReadOnlyDictionary<string, IInspectorPropertyInfo> IInspectorSchema.Properties
            => EmptyProperties;

        bool IInspectorSchema.TryGet(string name, out IInspectorPropertyInfo? property)
        {
            property = null;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => EmptyProperties.Values.GetEnumerator();

        IEnumerator<IInspectorPropertyInfo> IEnumerable<IInspectorPropertyInfo>.GetEnumerator()
            => EmptyProperties.Values.GetEnumerator();
    }
}