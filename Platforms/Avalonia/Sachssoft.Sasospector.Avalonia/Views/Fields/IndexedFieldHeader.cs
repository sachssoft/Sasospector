using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views.Fields
{
    public class IndexedFieldHeader : FieldHeaderBase
    {
        public static readonly StyledProperty<int> IndexProperty =
            AvaloniaProperty.Register<FieldHeaderBase, int>(nameof(Index));
        
        public int Index
        {
            get => GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public override bool Match(int index, Type? dataType, object? dataValue)
        {
            return index == Index;
        }
    }
}
