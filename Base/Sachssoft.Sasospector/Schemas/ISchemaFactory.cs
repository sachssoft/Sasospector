namespace Sachssoft.Sasospector.Schemas
{
    public interface ISchemaFactory
    {
        bool IsMatch(object? model);

        IInspectorSchemaProvider Create(object model);
    }
}
