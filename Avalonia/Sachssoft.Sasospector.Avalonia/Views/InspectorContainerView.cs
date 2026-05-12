using Avalonia.Collections;
using Avalonia.Metadata;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorContainerView : InspectorItem
    {
        private readonly AvaloniaList<InspectorItem> _items = new();
        protected override Type StyleKeyOverride { get; } = typeof(InspectorContainerView);

        [Content]
        public AvaloniaList<InspectorItem> Items => _items;
    }
}
