using Avalonia;
using Avalonia.Controls.Primitives;
using Sachssoft.Sasospector.Schemas;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorItem : TemplatedControl
    {
        public static readonly StyledProperty<IInspectorSchema> SchemaProperty =
            AvaloniaProperty.Register<InspectorItem, IInspectorSchema>(
                nameof(Schema));

        public static readonly StyledProperty<object?> HeaderProperty =
            AvaloniaProperty.Register<InspectorItem, object?>(nameof(Header));

        public IInspectorSchema Schema
        {
            get => GetValue(SchemaProperty);
            set => SetValue(SchemaProperty, value);
        }

        public object? Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
    }
}
