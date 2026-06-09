using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Purposes
{
    public class ObjectPickerPurpose : IInspectorPropertyPurpose
    {
        public ObjectPickerPurpose() { }

        public bool IsMatch(Type propertyType)
        {
            return !propertyType.IsValueType && 
                !propertyType.IsArray;
        }

        public bool IsEquivalentTo(IInspectorPropertyPurpose? other)
        {
            if (other is not ObjectPickerPurpose otherPurpose)
                return false;

            return true;
        }
    }
}
