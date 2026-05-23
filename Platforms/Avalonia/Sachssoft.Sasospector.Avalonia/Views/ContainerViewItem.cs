using Avalonia.Collections;
using Avalonia.Metadata;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class ContainerViewItem : InspectorItemBase
    {
        private readonly AvaloniaList<InspectorItemBase> _items = new();

        protected override Type StyleKeyOverride { get; } = typeof(ContainerViewItem);

        [Content]
        public AvaloniaList<InspectorItemBase> Items => _items;
    }
}
