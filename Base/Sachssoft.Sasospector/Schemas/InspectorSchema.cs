using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Schemas
{
    public class InspectorSchema : IInspectorSchema, IDisposable
    {
        private readonly object _owner;
        private readonly IReadOnlyDictionary<string, IInspectorPropertyInfo> _properties;

        private bool _disposed;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        public InspectorSchema(object owner, IEnumerable<IInspectorPropertyInfo> properties)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            var dict = new Dictionary<string, IInspectorPropertyInfo>();

            foreach (var property in properties)
            {
                if (property == null)
                    throw new InvalidOperationException("Property cannot be null.");

                var name = property.Name;
                if (string.IsNullOrWhiteSpace(name))
                    throw new InvalidOperationException("Property name is invalid.");

                if (!dict.TryAdd(name, property))
                    throw new InvalidOperationException($"Duplicate property '{name}'.");

                property.ValueChanging += Property_ValueChanging;
                property.ValueChanged += Property_ValueChanged;
            }

            _properties = dict;
        }

        internal InspectorSchema(
            object owner,
            Dictionary<string, Func<InspectorSchema, IInspectorPropertyInfo>> propertyFactories)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));

            var dict = new Dictionary<string, IInspectorPropertyInfo>(propertyFactories.Count);

            foreach (var kv in propertyFactories)
            {
                var factory = kv.Value;
                var property = factory.Invoke(this);
                var name = property.Name ?? kv.Key;

                dict[name] = property;

                property.ValueChanging += Property_ValueChanging;
                property.ValueChanged += Property_ValueChanged;
            }

            _properties = dict;
        }

        public object Owner => _owner;

        public IReadOnlyDictionary<string, IInspectorPropertyInfo> Properties => _properties;

        public bool TryGet(string name, out IInspectorPropertyInfo? property)
            => _properties.TryGetValue(name, out property);

        public bool TryGet<T>(string name, out InspectorPropertyInfo? property)
        {
            if (_properties.TryGetValue(name, out var entry) &&
                entry is InspectorPropertyInfo typed)
            {
                property = typed;
                return true;
            }

            property = null;
            return false;
        }

        public IEnumerator<IInspectorPropertyInfo> GetEnumerator()
            => _properties.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(InspectorSchema));

            foreach (var property in _properties.Values)
            {
                property.ValueChanging -= Property_ValueChanging;
                property.ValueChanged -= Property_ValueChanged;
            }

            PropertyChanged = null;
            PropertyChanging = null;

            _disposed = true;
        }

        private void Property_ValueChanging(object? sender, InspectorPropertyChangingEventArgs e)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(e.Property.Name));
        }

        private void Property_ValueChanged(object? sender, InspectorPropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.Property.Name));
        }
    }
}
