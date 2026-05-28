using System;

namespace Sachssoft.Sasospector
{
    public delegate object? GetterDelegate(object source, Type propertyType);

    public delegate object? GetterDelegate<TSource>(TSource source, Type propertyType)
         where TSource : class;
}
