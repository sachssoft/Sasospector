namespace Sachssoft.Sasospector.Schemas
{
    public interface IInspectorSchemaSource
    {
        IInspectorSchema Resolve(object? model);

        void RequestSynchronize(object? model, string propertyName);
    }
}
