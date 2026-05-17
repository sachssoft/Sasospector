using System;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Schemas
{
    public sealed class SchemaFactorySelector<TModel> : IInspectorSchemaProvider, INotifyPropertyChanged
        where TModel : class
    {
        private readonly ISchemaFactory[] _factories;
        private IInspectorSchema _schema;

        public event PropertyChangedEventHandler? PropertyChanged;

        public SchemaFactorySelector(params ISchemaFactory[] factories)
        {
            _factories = factories ?? throw new ArgumentNullException(nameof(factories));

            if (_factories.Length == 0)
                throw new ArgumentException("At least one factory is required.", nameof(factories));

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

        public void Resolve(TModel? model)
        {
            if (model is null)
            {
                Schema = EmptyInspectorSchema.Instance;
                return;
            }

            foreach (var factory in _factories)
            {
                if (!factory.IsMatch(model))
                    continue;

                var provider = factory.Create(model);

                if (provider?.Schema.Owner == model)
                {
                    Schema = provider.Schema;
                    return;
                }
            }

            Schema = EmptyInspectorSchema.Instance;
        }
    }
}