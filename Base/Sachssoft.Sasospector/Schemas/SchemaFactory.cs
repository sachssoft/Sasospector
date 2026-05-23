using System;

namespace Sachssoft.Sasospector.Schemas
{
    public class SchemaFactory<TModel> : ISchemaFactory
        where TModel : class
    {
        private readonly Func<IInspectorSchemaSource> _factory;

        public SchemaFactory(Func<IInspectorSchemaSource> factory)
        {
            _factory = factory;
        }

        public IInspectorSchemaSource Create(TModel? model)
        {
            return _factory();
        }

        IInspectorSchemaSource ISchemaFactory.Create(object? model)
        {
            return _factory();
        }

        public bool IsMatch(object? model)
        {
            return model is TModel;
        }
    }
}
