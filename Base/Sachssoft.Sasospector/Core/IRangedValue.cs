using System;

namespace Sachssoft.Sasospector
{
    public interface IRangedValue
    {

        object Value { get; }

        object MinValue { get; }

        object MaxValue { get; }

        Type ValueType { get; }

        bool IsBounded { get; }
    }
}
