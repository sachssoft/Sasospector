using System;
using System.Numerics;

namespace Sachssoft.Sasospector
{
    public readonly struct BoundedValue<T> : IRangedValue
        where T : struct, IMinMaxValue<T>, INumber<T>
    {
        public BoundedValue(T value, T min, T max)
        {
            MinValue = min;
            MaxValue = max;

            Value = Clamp(value, min, max);
        }

        public T Value { get; }

        object IRangedValue.Value => Value;

        public T MinValue { get; }

        object IRangedValue.MinValue => MinValue;

        public T MaxValue { get; }

        object IRangedValue.MaxValue => MaxValue;

        Type IRangedValue.ValueType { get; } = typeof(T);

        public bool IsBounded { get; } = true;

        private static T Clamp(T value, T min, T max)
        {
            if (min > max)
                (min, max) = (max, min);

            return value < min ? min : (value > max ? max : value);
        }
    }
}