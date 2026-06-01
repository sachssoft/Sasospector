using System;

namespace Sachssoft.Sasospector.Purposes
{
    public sealed class UriPurpose : IInspectorPropertyPurpose
    {
        public bool IsMatch(Type propertyType)
        {
            return propertyType == typeof(string)
                || propertyType == typeof(Uri);
        }

        public bool IsEquivalentTo(IInspectorPropertyPurpose? other)
        {
            return other is UriPurpose;
        }
    }
}