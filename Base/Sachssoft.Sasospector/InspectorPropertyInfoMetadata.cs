using Sachssoft.Sasospector.Constraints;
using System;
using System.Collections.Generic;


namespace Sachssoft.Sasospector
{
    public class InspectorPropertyInfoMetadata
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

        public Type? CustomEditorType { get; init; }

        public string? EditorKind { get; init; }

        public IReadOnlyList<IInspectorConstraint>? Constraints { get; init; }

        public string? CategoryName { get; init; }

        public virtual Type? EditorValueType => null;

        public virtual object? DefaultValue => null;

        public virtual string? DisplayName => null;
    }
}
