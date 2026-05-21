
using Avalonia;
using Avalonia.Metadata;
using Sachssoft.Sasospector.Views.Editors;

namespace Sachssoft.Sasospector.Views
{
    public class CustomEditorContent : PropertyEditorBase
    {

        public static readonly StyledProperty<object?> ContentProperty =
            AvaloniaProperty.Register<CustomEditorContent, object?>(nameof(Content));

        [Content]
        public object? Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
    }
}
