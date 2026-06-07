using System;

namespace Sachssoft.Sasospector.Purposes
{
    public interface IInspectorPropertyPurpose
    {
        bool IsMatch(Type propertyType);

        bool IsEquivalentTo(IInspectorPropertyPurpose? other);
    }
}
