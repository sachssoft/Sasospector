using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Purposes
{
    public interface IInspectorPropertyPurpose 
    {
        bool IsMatch(Type propertyType);

        bool IsEquivalentTo(IInspectorPropertyPurpose? other);
    }
}
