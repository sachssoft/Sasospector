using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Sachssoft.Sasospector.Schemas;
using Sachssoft.Sasospector.Views.Fields;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sachssoft.Sasospector.Views
{
    public abstract class InspectorItemBase : TemplatedControl
    {
        private IInspectorPropertyInfo? _property;
        private object? _selectedFieldValue;
        private IInspectorSchema? _schema;

        private bool _visualTreeAttached;
        private IInspectorSchema? _activeSchema;
        private object? _activeModel;

        public static readonly StyledProperty<IInspectorSchemaSource?> SchemaSourceProperty =
            AvaloniaProperty.Register<InspectorItemBase, IInspectorSchemaSource?>(nameof(SchemaSource));

        public static readonly StyledProperty<object?> ModelProperty =
            AvaloniaProperty.Register<InspectorItemBase, object?>(nameof(Model));

        public static readonly DirectProperty<InspectorItemBase, IInspectorSchema?> SchemaProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, IInspectorSchema?>(
                nameof(Schema),
                o => o.Schema,
                (o, v) => o.Schema = v);

        public static readonly StyledProperty<object?> HeaderProperty =
            AvaloniaProperty.Register<InspectorItemBase, object?>(nameof(Header));

        public static readonly StyledProperty<bool> IsHeaderVisibleProperty =
            AvaloniaProperty.Register<InspectorItemBase, bool>(nameof(IsHeaderVisible));

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<InspectorItemBase, string?>(nameof(PropertyName));

        public static readonly DirectProperty<InspectorItemBase, IInspectorPropertyInfo?> PropertyProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, IInspectorPropertyInfo?>(
                nameof(Property),
                o => o._property,
                (o, v) => o.Property = v);

        public static readonly StyledProperty<IList<FieldHeaderBase>?> FieldHeadersProperty =
            AvaloniaProperty.Register<InspectorItemBase, IList<FieldHeaderBase>?>(nameof(FieldHeaders));

        public static readonly StyledProperty<IDataTemplate?> FieldHeaderTemplateProperty =
            AvaloniaProperty.Register<InspectorItemBase, IDataTemplate?>(nameof(FieldHeaderTemplate));

        public static readonly DirectProperty<InspectorItemBase, object?> SelectedFieldValueProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, object?>(
                nameof(SelectedFieldValue),
                o => o._selectedFieldValue,
                (o, v) => o._selectedFieldValue = v,
                defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
            AvaloniaProperty.Register<InspectorItemBase, IDataTemplate?>(nameof(ItemTemplate));

        public static readonly StyledProperty<bool> AutoSynchronizationProperty =
            AvaloniaProperty.Register<InspectorItemBase, bool>(nameof(AutoSynchronization));

        public static readonly DirectProperty<InspectorItemBase, IInspectorSchema?> ActiveSchemaProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, IInspectorSchema?>(
                nameof(ActiveSchema),
                o => o._activeSchema,
                (o, v) => o._activeSchema = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

        public static readonly DirectProperty<InspectorItemBase, object?> ActiveModelProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, object?>(
                nameof(ActiveModel),
                o => o._activeModel,
                (o, v) => o._activeModel = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

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

        public IInspectorSchema? Schema
        {
            get => _schema;
            private set => SetAndRaise(SchemaProperty, ref _schema, value);
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

        public IInspectorSchema? ActiveSchema
        {
            get => _activeSchema;
            private set => SetAndRaise(ActiveSchemaProperty, ref _activeSchema, value);
        }

        public object? ActiveModel
        {
            get => _activeModel;
            private set => SetAndRaise(ActiveModelProperty, ref _activeModel, value);
        }

        #region Lifecycle

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            _visualTreeAttached = true;

            ApplySchema();
            UpdateSchema();
            UpdateProperty();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _visualTreeAttached = false;
            _activeSchema = null;
            Property = null;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (!_visualTreeAttached)
                return;

            if (change.Property == SchemaSourceProperty ||
                change.Property == ModelProperty)
            {
                _activeSchema = null;
                ApplySchema();
                UpdateSchema();
            }
            else if (change.Property == SchemaProperty ||
                change.Property == PropertyNameProperty)
            {
                UpdateProperty();
            }
        }

        #endregion

        #region Core 

        internal protected virtual void OnParentSchemaChanged() { }

        protected virtual void OnSchemaPropertyChanging(PropertyChangingEventArgs e) { }

        protected virtual void OnSchemaPropertyChanged(PropertyChangedEventArgs e) { }

        private void CurrentSchema_PropertyChanging(object? sender, PropertyChangingEventArgs e)
            => OnSchemaPropertyChanging(e);

        private void CurrentSchema_PropertyChanged(object? sender, PropertyChangedEventArgs e)
            => OnSchemaPropertyChanged(e);

        private void ApplySchema()
        {
            if (SchemaSource == null || Model == null)
            {
                Schema = null;
                return;
            }

            Schema = SchemaSource.Resolve(Model);
        }

        // Durchsuche nach Schema und Model in der aktuellen Instanz und den Eltern,
        // bis beides gefunden wurde oder keine Eltern mehr da sind.
        private void UpdateSchema()
        {
            IInspectorSchema? schema = Schema;
            object? model = Model;

            var parent = Parent;

            while (parent != null)
            {
                if (parent is ContainerViewItem container)
                {
                    schema ??= container.ItemSchema;
                    model ??= container.ItemModel;
                }

                if (schema != null || model != null)
                    break;

                parent = parent.Parent;
            }

            ActiveSchema = schema;
            ActiveModel = model;
        }

        protected void UpdateProperty()
        {
            IInspectorPropertyInfo? newProperty = null;

            if (_activeSchema != null &&
                PropertyName != null &&
                _activeSchema.Properties.TryGetValue(PropertyName, out var propertyInfo))
            {
                newProperty = PreferredPropertyOverride(propertyInfo);
            }

            // ACHTUNG! ReferenceEquals NICHT ERLAUBT!! 
            // Ständige Aktualisierung WICHTIG

            var oldProperty = Property;
            Property = newProperty;

            if (oldProperty != null)
            {
                OnUpdatePropertyExit(oldProperty);
            }

            if (newProperty != null)
            {
                OnUpdatePropertyEnter(newProperty);
            }
        }

        protected virtual void OnUpdatePropertyEnter(IInspectorPropertyInfo property)
            => System.Diagnostics.Debug.WriteLine($"ENTER: {property?.Name ?? "<null>"}");

        protected virtual void OnUpdatePropertyExit(IInspectorPropertyInfo property)
            => System.Diagnostics.Debug.WriteLine($"EXIT: {property?.Name ?? "<null>"}");

        #endregion

        #region Extension Points

        protected virtual IInspectorPropertyInfo? PreferredPropertyOverride(IInspectorPropertyInfo? basePropertyInfo)
            => basePropertyInfo;

        #endregion
    }
}