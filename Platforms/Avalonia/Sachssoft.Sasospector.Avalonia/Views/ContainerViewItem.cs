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
        private IInspectorSchema? _schema;

        public static readonly StyledProperty<IInspectorSchemaSource?> SchemaSourceProperty =
            AvaloniaProperty.Register<ContainerViewItem, IInspectorSchemaSource?>(nameof(SchemaSource));

        public static readonly StyledProperty<object?> ModelProperty =
            AvaloniaProperty.Register<ContainerViewItem, object?>(nameof(Model));

        public static readonly DirectProperty<ContainerViewItem, IInspectorSchema?> SchemaProperty =
            AvaloniaProperty.RegisterDirect<ContainerViewItem, IInspectorSchema?>(
                nameof(Schema),
                o => o.Schema,
                (o, v) => o.Schema = v);

        [Content]
        public AvaloniaList<InspectorItemBase> Items => _items;

        protected override Type StyleKeyOverride { get; } = typeof(ContainerViewItem);

        public IInspectorSchemaSource? SchemaSource
        {
            get => GetValue(SchemaSourceProperty);
            set => SetValue(SchemaSourceProperty, value);
        }

        public object? Model
        {
            get => GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public IInspectorSchema? Schema
        {
            get => _schema;
            private set => SetAndRaise(SchemaProperty, ref _schema, value);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == SchemaSourceProperty ||
                change.Property == ModelProperty)
            {
                ApplySchema();
                NotifyChildrenSchemaChanged();
            }
        }

        private void ApplySchema()
        {
            if (SchemaSource == null || Model == null)
            {
                Schema = null;
                return;
            }

            Schema = SchemaSource.Resolve(Model);
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
