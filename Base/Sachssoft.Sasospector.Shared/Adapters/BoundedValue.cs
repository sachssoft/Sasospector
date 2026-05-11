using System.Numerics;

namespace Sachssoft.Sasospector.Adapters
{
    public readonly struct BoundedValue<T>
        where T : struct, IMinMaxValue<T>, INumber<T>
    {
        public BoundedValue(T value, T min, T max)
        {
            MinValue = min;
            MaxValue = max;

            Value = Clamp(value, min, max);
        }

        public T Value { get; }

        public T MinValue { get; }

        public T MaxValue { get; }

        public T ClampedValue => Clamp(Value, MinValue, MaxValue);

        public bool IsMinimum => Value <= MinValue;

        public bool IsMaximum => Value >= MaxValue;

        public BoundedValue<T> WithValue(T value)
            => new BoundedValue<T>(value, MinValue, MaxValue);

        public BoundedValue<T> WithRange(T min, T max)
            => new BoundedValue<T>(Value, min, max);

        private static T Clamp(T value, T min, T max)
        {
            if (min > max)
                (min, max) = (max, min);

            return value < min ? min : (value > max ? max : value);
        }
    }
}