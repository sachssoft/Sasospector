using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Schemas
{
    public sealed class StaticSchemaSelector : IInspectorSchemaProvider, INotifyPropertyChanged
    {
        private readonly IInspectorSchemaProvider[] _schemaProviders;
        private IInspectorSchema _schema;

        public event PropertyChangedEventHandler? PropertyChanged;

        public StaticSchemaSelector(params IInspectorSchemaProvider[] schemaProviders)
        {
            _schemaProviders = schemaProviders ?? throw new ArgumentNullException(nameof(schemaProviders));

            if (_schemaProviders.Length == 0)
                throw new ArgumentException("At least one schema provider is required.", nameof(schemaProviders));

            _schema = EmptyInspectorSchema.Instance;
        }

        public IInspectorSchema Schema
        {
            get => _schema;
            private set
            {
                if (ReferenceEquals(_schema, value))
                    return;

                _schema = value ?? EmptyInspectorSchema.Instance;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Schema)));
            }
        }

        public void Resolve(object? model)
        {
            if (model is null)
            {
                Schema = EmptyInspectorSchema.Instance;
                return;
            }

            foreach (var provider in _schemaProviders)
            {
                var schema = provider.Schema;

                if (schema.Owner == model)
                {
                    Schema = schema;
                    return;
                }
            }

            Schema = EmptyInspectorSchema.Instance;
        }
    }
}