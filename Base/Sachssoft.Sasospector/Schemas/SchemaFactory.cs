using System;

namespace Sachssoft.Sasospector.Schemas
{
    public class SchemaFactory<T> : ISchemaFactory
        where T : class
    {
        private readonly Func<T, IInspectorSchemaProvider> _factory;

        public SchemaFactory(Func<T, IInspectorSchemaProvider> factory)
        {
            _factory = factory;
        }

        public IInspectorSchemaProvider Create(T model)
        {
            return _factory(model);
        }

        IInspectorSchemaProvider ISchemaFactory.Create(object model)
        {
            return _factory((T)model);
        }

        public bool IsMatch(object? model)
        {
            return model is T;
        }
    }
}
