using Avalonia;
using Avalonia.Collections;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Views
{
    public class ContainerViewItem : InspectorItem
    {
        private readonly AvaloniaList<InspectorItem> _items = new();

        protected override Type StyleKeyOverride { get; } = typeof(ContainerViewItem);

        public ContainerViewItem()
        {
        }

        [Content]
        public AvaloniaList<InspectorItem> Items => _items;

        //protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        //{
        //    base.OnAttachedToVisualTree(e);

        //    if (DataContext == null && e.Root is StyledElement se)
        //    {
        //        DataContext = se.DataContext;
        //    }
        //}

        //protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        //{
        //    base.OnPropertyChanged(change);

        //    if (change.Property == DataContextProperty)
        //    {
        //        foreach (var item in Items)
        //        {
        //            item.DataContext = DataContext;
        //        }
        //    }
        //}
    }
}
