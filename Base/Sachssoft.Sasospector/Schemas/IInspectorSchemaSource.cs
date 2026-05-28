namespace Sachssoft.Sasospector.Schemas
{
    public interface IInspectorSchemaSource
    {
        IInspectorSchema Resolve(object? model);
    }
}
