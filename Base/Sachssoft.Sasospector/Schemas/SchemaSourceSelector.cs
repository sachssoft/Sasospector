using System;

namespace Sachssoft.Sasospector.Schemas
{
    public sealed class SchemaSourceSelector<T> : IInspectorSchemaSource
        where T : class
    {
        private readonly ISchemaFactory[] _factories;

        public SchemaSourceSelector(params ISchemaFactory[] factories)
        {
            _factories = factories
                ?? throw new ArgumentNullException(nameof(factories));

            if (_factories.Length == 0)
            {
                throw new ArgumentException(
                    "At least one schema factory is required.",
                    nameof(factories));
            }
        }

        public IInspectorSchema Resolve(T? model)
        {
            if (model is null)
                return EmptyInspectorSchema.Instance;

            foreach (var factory in _factories)
            {
                if (factory.IsMatch(model))
                {
                    var provider = factory.Create(model)
                        ?? throw new InvalidOperationException(
                            $"Factory '{factory.GetType().FullName}' returned null.");

                    return provider.Resolve(model);
                }
            }

            throw new InvalidOperationException(
                $"No schema factory found for model type '{model.GetType().FullName}'.");
        }

        IInspectorSchema IInspectorSchemaSource.Resolve(object? model)
        {
            if (model is null)
                return EmptyInspectorSchema.Instance;

            if (model is T typed)
                return Resolve(typed);

            throw new InvalidCastException(
                $"Expected model of type '{typeof(T).FullName}', but got '{model.GetType().FullName}'.");
        }
    }
}