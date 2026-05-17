using System.Numerics;

namespace Sachssoft.Sasospector.Constraints
{
    public class IncrementConstraint<T> : InspectorConstraintBase<T>
        where T : struct, IMinMaxValue<T>, INumber<T>
    {
        public T SmallChange { get; init; }

        public T LargeChange { get; init; }
    }
}
