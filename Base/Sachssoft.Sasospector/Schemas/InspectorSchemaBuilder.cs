using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Schemas
{
    public class InspectorSchemaBuilder
    {
        private readonly Dictionary<string, Func<InspectorSchema, IInspectorPropertyInfo>> _propertyFactories = new();

        private bool _built;

        public InspectorSchemaBuilder()
        {
        }

        public void AddProperty(
            Type type,
            string name,
            GetterDelegate getter,
            SetterDelegate? setter = null,
            InspectorPropertyInfoMetadata? metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Property name cannot be null or empty.", nameof(name));

            if (getter is null)
                throw new ArgumentNullException(nameof(getter));

            if (_propertyFactories.ContainsKey(name))
                throw new InvalidOperationException($"Property '{name}' is already registered.");

            metadata ??= CreatePropertyMeta() ?? new InspectorPropertyInfoMetadata();
            _propertyFactories[name] = (scheme) => CreateProperty(scheme, name, type, getter, setter, metadata);
        }

        protected virtual InspectorPropertyInfoMetadata CreatePropertyMeta()
            => new InspectorPropertyInfoMetadata();

        protected virtual InspectorPropertyInfo CreateProperty(
            InspectorSchema scheme,
            string name,
            Type type,
            GetterDelegate getter,
            SetterDelegate? setter,
            InspectorPropertyInfoMetadata metadata)
        {
            return new InspectorPropertyInfo(scheme, name, type, getter, setter, metadata);
        }

        public InspectorSchema Build()
        {
            if (_built)
                throw new InvalidOperationException("Already built.");

            _built = true;

            return new InspectorSchema(_propertyFactories);
        }
    }
}