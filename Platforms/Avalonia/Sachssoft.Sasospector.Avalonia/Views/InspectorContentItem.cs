using Avalonia;
using Avalonia.Metadata;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorContentItem : InspectorItem
    {
        public static readonly StyledProperty<object?> ContentProperty =
            AvaloniaProperty.Register<InspectorContentItem, object?>(nameof(Content));

        protected override Type StyleKeyOverride { get; } = typeof(InspectorContentItem);

        [Content]
        public object? Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
    }
}
