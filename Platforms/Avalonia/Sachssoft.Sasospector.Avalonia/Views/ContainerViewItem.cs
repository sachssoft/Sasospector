using Avalonia;
using Avalonia.Collections;
using Avalonia.Metadata;
using Sachssoft.Sasospector.Schemas;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class ContainerViewItem : InspectorItemBase
    {
        private readonly AvaloniaList<InspectorItemBase> _items = new();
        private IInspectorSchema? _itemSchema;

        public static readonly StyledProperty<IInspectorSchemaSource?> ItemSchemaSourceProperty =
            AvaloniaProperty.Register<ContainerViewItem, IInspectorSchemaSource?>(nameof(ItemSchemaSource));

        public static readonly StyledProperty<object?> ItemModelProperty =
            AvaloniaProperty.Register<ContainerViewItem, object?>(nameof(ItemModel));

        public static readonly DirectProperty<ContainerViewItem, IInspectorSchema?> ItemSchemaProperty =
            AvaloniaProperty.RegisterDirect<ContainerViewItem, IInspectorSchema?>(
                nameof(ItemSchema),
                o => o.ItemSchema,
                (o, v) => o.ItemSchema = v);

        [Content]
        public AvaloniaList<InspectorItemBase> Items => _items;

        protected override Type StyleKeyOverride { get; } = typeof(ContainerViewItem);

        public IInspectorSchemaSource? ItemSchemaSource
        {
            get => GetValue(ItemSchemaSourceProperty);
            set => SetValue(ItemSchemaSourceProperty, value);
        }

        public object? ItemModel
        {
            get => GetValue(ItemModelProperty);
            set => SetValue(ItemModelProperty, value);
        }

        public IInspectorSchema? ItemSchema
        {
            get => _itemSchema;
            private set => SetAndRaise(ItemSchemaProperty, ref _itemSchema, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ItemSchemaSourceProperty ||
                change.Property == ItemModelProperty)
            {
                ApplySchema();
                NotifyChildrenSchemaChanged();
            }
        }

        private void ApplySchema()
        {
            if (ItemSchemaSource == null || ItemModel == null)
            {
                ItemSchema = null;
                return;
            }

            ItemSchema = ItemSchemaSource.Resolve(ItemModel);
        }

        private void NotifyChildrenSchemaChanged()
        {
            foreach (var child in LogicalChildren)
            {
                if (child is InspectorItemBase item)
                {
                    item.OnParentSchemaChanged();
                }
            }
        }
    }
}
