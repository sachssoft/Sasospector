namespace Sachssoft.Sasospector.Schemas
{
    public interface ISchemaFactory
    {
        bool IsMatch(object? model);

        IInspectorSchemaSource Create(object? model);
    }
}
