using System;
using System.Numerics;

namespace Sachssoft.Sasospector
{
    public readonly struct UnboundedValue<T> : IRangedValue
        where T : struct, INumber<T>
    {
        public UnboundedValue(T value)
        {
            Value = value;
        }

        public T Value { get; }

        object IRangedValue.Value => Value;

        object IRangedValue.MinValue => default(T)!;

        object IRangedValue.MaxValue => default(T)!;

        public bool IsBounded { get; } = false;

        Type IRangedValue.ValueType { get; } = typeof(T);
    }
}