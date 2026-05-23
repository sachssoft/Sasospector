using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Sachssoft.Sasospector.Schemas;
using Sachssoft.Sasospector.Views.Fields;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Views
{
    public abstract class InspectorItemBase : TemplatedControl
    {
        private IInspectorPropertyInfo? _property;
        private object? _selectedFieldValue;

        public static readonly StyledProperty<IInspectorSchemaSource?> SchemaSourceProperty =
            AvaloniaProperty.Register<InspectorItemBase, IInspectorSchemaSource?>(nameof(SchemaSource));

        public static readonly StyledProperty<object?> ModelProperty =
            AvaloniaProperty.Register<InspectorItemBase, object?>(nameof(Model));

        public static readonly StyledProperty<object?> HeaderProperty =
            AvaloniaProperty.Register<InspectorItemBase, object?>(nameof(Header));

        public static readonly StyledProperty<bool> IsHeaderVisibleProperty =
            AvaloniaProperty.Register<InspectorItemBase, bool>(nameof(IsHeaderVisible));

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<InspectorItemBase, string?>(nameof(PropertyName));

        public static readonly DirectProperty<InspectorItemBase, IInspectorPropertyInfo?> PropertyProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, IInspectorPropertyInfo?>(
                nameof(Property),
                o => o.Property,
                (o, v) => o.Property = v);

        public static readonly StyledProperty<IList<FieldHeaderBase>?> FieldHeadersProperty =
            AvaloniaProperty.Register<InspectorItemBase, IList<FieldHeaderBase>?>(nameof(FieldHeaders));

        public static readonly StyledProperty<IDataTemplate?> FieldHeaderTemplateProperty =
            AvaloniaProperty.Register<InspectorItemBase, IDataTemplate?>(nameof(FieldHeaderTemplate));

        public static readonly DirectProperty<InspectorItemBase, object?> SelectedFieldValueProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, object?>(
                nameof(SelectedFieldValue),
                o => o.SelectedFieldValue,
                (o, v) => o.SelectedFieldValue = v,
                defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
            AvaloniaProperty.Register<InspectorItemBase, IDataTemplate?>(nameof(ItemTemplate));

        public static readonly StyledProperty<bool> AutoSynchronizationProperty =
            AvaloniaProperty.Register<InspectorItemBase, bool>(nameof(AutoSynchronization));

        public record SchemaResult(IInspectorSchemaSource? SchemaSource, object? Model);

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public IInspectorPropertyInfo? Property
        {
            get => _property;
            protected set => SetAndRaise(PropertyProperty, ref _property, value);
        }

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

        protected SchemaResult ResolveSchemaContext()
        {
            IInspectorSchemaSource? schemaSource = null;
            object? model = null;

            if (SchemaSource != null)
                schemaSource = SchemaSource;

            if (Model != null)
                model = Model;

            var parent = Parent;

            while (parent != null)
            {
                if (parent is SectionView section)
                {
                    if (schemaSource == null && section.SchemaSource != null)
                        schemaSource = section.SchemaSource;

                    if (model == null && section.Model != null)
                        model = section.Model;
                }

                if (schemaSource != null && model != null)
                    break;

                parent = parent.Parent;
            }

            return new SchemaResult(schemaSource, model);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == PropertyNameProperty ||
                change.Property == SchemaSourceProperty)
            {
                InvalidateProperty();
            }
        }

        protected virtual IInspectorPropertyInfo? PreferredPropertyOverride(IInspectorPropertyInfo? basePropertyInfo)
            => basePropertyInfo;

        protected virtual void OnPropertyInvalidated(IInspectorPropertyInfo propertyInfo) { }

        protected void InvalidateProperty()
        {
            if (string.IsNullOrEmpty(PropertyName) || SchemaSource == null)
                return;

            var schema = SchemaSource.Resolve(DataContext);

            if (schema.Properties.TryGetValue(PropertyName, out var propertyInfo))
            {
                Property = PreferredPropertyOverride(propertyInfo);

                if (Property != null)
                    OnPropertyInvalidated(Property);
            }
            else
            {
                Property = null;
            }
        }
    }
}