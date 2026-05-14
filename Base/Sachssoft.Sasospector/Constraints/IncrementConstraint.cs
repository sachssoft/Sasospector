using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Constraints
{
    public class IncrementConstraint<T> : InspectorConstraintBase<T>
        where T : struct, IMinMaxValue<T>, INumber<T>
    {
        public T SmallChange { get; init; }

        public T LargeChange { get; init; }
    }
}
