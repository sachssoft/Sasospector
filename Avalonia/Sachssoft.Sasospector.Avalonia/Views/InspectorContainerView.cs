using Avalonia.Collections;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
