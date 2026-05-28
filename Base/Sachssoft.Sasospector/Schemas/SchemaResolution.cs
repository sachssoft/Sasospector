namespace Sachssoft.Sasospector.Schemas
{
    public sealed class SchemaResolution
    {
        private readonly IInspectorSchemaSource? _schemaSource;
        private readonly object? _model;
        private readonly IInspectorSchema? _schema;

        public SchemaResolution(
            IInspectorSchemaSource? schemaSource,
            object? model)
        {
            _schemaSource = schemaSource;
            _model = model;

            _schema = model != null
                ? schemaSource?.Resolve(model)
                : null;
        }

        public IInspectorSchemaSource? SchemaSource => _schemaSource;

        public object? Model => _model;

        public IInspectorSchema? Schema => _schema;

        public bool IsValid => _schema != null;

        public IInspectorPropertyInfo? FindProperty(string? propertyName)
        {
            if (_schema == null || string.IsNullOrEmpty(propertyName))
                return null;

            return _schema.Properties.TryGetValue(
                propertyName,
                out var property)
                ? property
                : null;
        }
    }
}