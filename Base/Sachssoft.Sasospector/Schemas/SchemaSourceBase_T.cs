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

        IInspectorSchema IInspectorSchemaSource.Resolve(object? model)
        {
            if (model is null)
                return EmptyInspectorSchema.Instance;

            return Resolve(EnsureTypedModel(model));
        }

        // Fordert die Synchronisation eines Schemas an, nachdem sich eine Model-Eigenschaft geändert hat.
        // Das Model enthält keine Notify-Logik (reine Datenhaltung), daher wird die Synchronisation
        // explizit über das Schema-System angestoßen.
        public void RequestSynchronize(TModel model, string propertyName)
        {
            if (_schemas.TryGetValue(GetSchemaKey(model), out var schema))
            {
                schema.RequestSynchronize(propertyName);
            }
        }

        void IInspectorSchemaSource.RequestSynchronize(object? model, string propertyName)
        {
            if (model is null)
                return;

            RequestSynchronize(EnsureTypedModel(model), propertyName);
        }

        protected virtual Type GetSchemaKey(TModel model)
            => model.GetType();

        protected abstract IInspectorSchema? CreateSchema(TModel model);

        private TModel EnsureTypedModel(object model)
        {
            if (model is not TModel typedModel)
                throw new InvalidCastException(
                    $"Expected '{typeof(TModel).FullName}', got '{model.GetType().FullName}'.");

            return typedModel;
        }
    }
}