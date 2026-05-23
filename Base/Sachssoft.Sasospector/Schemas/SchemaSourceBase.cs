using System;

namespace Sachssoft.Sasospector.Schemas
{
    public abstract class SchemaSourceBase<TModel> : IInspectorSchemaSource
        where TModel : class
    {
        public IInspectorSchema Resolve(TModel? model)
        {
            if (model is null)
                return EmptyInspectorSchema.Instance;

            return CreateSchema(model)
                   ?? EmptyInspectorSchema.Instance;
        }

        protected abstract IInspectorSchema? CreateSchema(TModel model);

        IInspectorSchema IInspectorSchemaSource.Resolve(object? model)
        {
            // null -> leeres Schema
            if (model is null)
                return EmptyInspectorSchema.Instance;

            // falscher Typ -> klare Exception
            if (model is not TModel typedModel)
            {
                throw new InvalidCastException(
                    $"Expected model of type '{typeof(TModel).FullName}', " +
                    $"but received '{model.GetType().FullName}'.");
            }

            return Resolve(typedModel);
        }
    }
}