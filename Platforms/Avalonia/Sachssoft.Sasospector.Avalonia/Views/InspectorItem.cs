using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Sachssoft.Sasospector.Schemas;
using Sachssoft.Sasospector.Views.Fields;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorItem : TemplatedControl
    {
        private IInspectorPropertyInfo? _property;
        private object? _selectedFieldValue;

        public static readonly StyledProperty<IInspectorSchema> SchemaProperty =
            AvaloniaProperty.Register<InspectorItem, IInspectorSchema>(
                nameof(Schema));

        public static readonly StyledProperty<object?> HeaderProperty =
            AvaloniaProperty.Register<InspectorItem, object?>(nameof(Header));

        public static readonly StyledProperty<bool> IsHeaderVisibleProperty =
            AvaloniaProperty.Register<InspectorItem, bool>(nameof(IsHeaderVisible));

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<InspectorItem, string?>(nameof(PropertyName));

        public static readonly DirectProperty<InspectorItem, IInspectorPropertyInfo?> PropertyProperty =
            AvaloniaProperty.RegisterDirect<InspectorItem, IInspectorPropertyInfo?>(
                nameof(Property),
                o => o.Property);

        public static readonly StyledProperty<IList<FieldHeaderBase>?> FieldHeadersProperty =
            AvaloniaProperty.Register<PropertyViewItem, IList<FieldHeaderBase>?>(nameof(FieldHeaders));

        public static readonly StyledProperty<IDataTemplate?> FieldHeaderTemplateProperty =
            AvaloniaProperty.Register<InspectorItem, IDataTemplate?>(nameof(FieldHeaderTemplate));

        public static readonly DirectProperty<InspectorItem, object?> SelectedFieldValueProperty =
            AvaloniaProperty.RegisterDirect<InspectorItem, object?>(
                nameof(SelectedFieldValue),
                o => o.SelectedFieldValue,
                (o, v) => o.SelectedFieldValue = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWayToSource);

        public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
            AvaloniaProperty.Register<InspectorItem, IDataTemplate?>(nameof(ItemTemplate));

        public static readonly StyledProperty<bool> AutoSynchronizationProperty =
            AvaloniaProperty.Register<InspectorItem, bool>(nameof(AutoSynchronization));

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        // Nur ReadOnly wichtig für Bindings
        public IInspectorPropertyInfo? Property
        {
            get => _property;
            protected set => SetAndRaise(PropertyProperty, ref _property, value);
        }

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

        public bool IsHeaderVisible
        {
            get => GetValue(IsHeaderVisibleProperty);
            set => SetValue(IsHeaderVisibleProperty, value);
        }

        public IList<FieldHeaderBase>? FieldHeaders
        {
            get => GetValue(FieldHeadersProperty);
            set => SetValue(FieldHeadersProperty, value);
        }

        public IDataTemplate? FieldHeaderTemplate
        {
            get => GetValue(FieldHeaderTemplateProperty);
            set => SetValue(FieldHeaderTemplateProperty, value);
        }

        public IDataTemplate? ItemTemplate
        {
            get => GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public bool AutoSynchronization
        {
            get => GetValue(AutoSynchronizationProperty);
            set => SetValue(AutoSynchronizationProperty, value);
        }

        public object? SelectedFieldValue
        {
            get => _selectedFieldValue;
            internal set => SetAndRaise(SelectedFieldValueProperty, ref _selectedFieldValue, value);
        }

        public virtual void Invalidate() { }
    }
}
