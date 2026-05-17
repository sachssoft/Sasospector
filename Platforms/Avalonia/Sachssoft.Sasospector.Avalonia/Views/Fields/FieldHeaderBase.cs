using Avalonia;
using Avalonia.Controls.Templates;
using System;

namespace Sachssoft.Sasospector.Views.Fields
{
    public abstract class FieldHeaderBase : AvaloniaObject
    {
        public static readonly StyledProperty<object?> HeaderProperty =
            AvaloniaProperty.Register<FieldHeaderBase, object?>(nameof(Header));

        public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
            AvaloniaProperty.Register<FieldHeaderBase, IDataTemplate?>(nameof(HeaderTemplate));

        public static readonly StyledProperty<object?> IconProperty =
            AvaloniaProperty.Register<FieldHeaderBase, object?>(nameof(Icon));

        public static readonly StyledProperty<FieldDisplayStyle> DisplayStyleProperty =
            AvaloniaProperty.Register<FieldHeaderBase, FieldDisplayStyle>(nameof(DisplayStyle));

        public FieldHeaderBase() { }

        public object? Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public IDataTemplate? HeaderTemplate
        {
            get => GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public object? Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public FieldDisplayStyle DisplayStyle
        {
            get => GetValue(DisplayStyleProperty);
            set => SetValue(DisplayStyleProperty, value);
        }

        public abstract bool Match(int index, Type? dataType, object? dataValue);
    }
}
