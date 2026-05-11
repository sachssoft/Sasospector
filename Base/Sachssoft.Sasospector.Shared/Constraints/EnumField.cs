using System;

namespace Sachssoft.Sasospector.Constraints
{
    public record EnumField<T>
        where T : struct, Enum
    {
        public T Value { get; init; }
    }
}