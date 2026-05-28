using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Schemas
{
    public abstract class SchemaSourceBase<TModel> : IInspectorSchemaSource
        where TModel : class
    {
        private readonly Dictionary<Type, IInspectorSchema> _schemas = new();

        public IInspectorSchema Resolve(TModel? model)
        {
            if (model is null)
                return EmptyInspectorSchema.Instance;

            var type = GetSchemaKey(model);

            if (_schemas.TryGetValue(type, out var schema))
                return schema;

            schema = CreateSchema(model) ?? EmptyInspectorSchema.Instance;

            _schemas[type] = schema;

            return schema;
        }

        protected virtual Type GetSchemaKey(TModel model)
            => model.GetType();

        protected abstract IInspectorSchema? CreateSchema(TModel model);

        IInspectorSchema IInspectorSchemaSource.Resolve(object? model)
        {
            if (model is null)
                return EmptyInspectorSchema.Instance;

            if (model is not TModel typedModel)
                throw new InvalidCastException(
                    $"Expected '{typeof(TModel).FullName}', got '{model.GetType().FullName}'.");

            return Resolve(typedModel);
        }
    }
}