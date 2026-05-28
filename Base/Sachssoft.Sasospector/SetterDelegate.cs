using System;

namespace Sachssoft.Sasospector
{
    public delegate void SetterDelegate(object source, Type propertyType, object? propertyValue);

    public delegate void SetterDelegate<TSource>(TSource source, Type propertyType, object? propertyValue)
        where TSource : class;
}
