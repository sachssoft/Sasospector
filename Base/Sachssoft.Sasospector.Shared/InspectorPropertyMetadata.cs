using Sachssoft.Sasosual.Avalonia.Controls;
using System;


namespace Sachssoft.Sasospector
{
    public class InspectorPropertyMetadata
    {
        public virtual bool Validate(object? value)
        {
            return true;
        }

        public virtual object? Coerce(object? value)
        {
            return value;
        }

        public bool IsReadOnly { get; internal init; }
        public Type? CustomEditorType { get; internal init; }
        public string[]? EditorKinds { get; internal init; }
        public IInspectorMetadataConstraint[]? Constraints { get; internal init; }

        public string? CategoryName { get; init; }

        public virtual Type? EditorValueType => null;

        public virtual object? DefaultValue => null;

        public virtual string? DisplayName => null;
    }
}
