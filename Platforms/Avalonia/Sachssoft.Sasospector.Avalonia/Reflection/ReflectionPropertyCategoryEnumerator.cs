using Avalonia;
using System;
using System.Collections;

namespace Sachssoft.Sasosual.Avalonia.Controls.Inspector.Reflection
{
    public class ReflectionPropertyCategoryEnumerator : AvaloniaObject, IEnumerable
    {
        public Type Type
        {
            get;
            set;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
