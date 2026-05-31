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
        private IInspectorSchema? _localSchema;

        private bool _visualTreeAttached;
        private IInspectorSchema? _resolvedSchema;
        private object? _resolvedModel;

        public static readonly StyledProperty<IInspectorSchemaSource?> LocalSchemaSourceProperty =
            AvaloniaProperty.Register<InspectorItemBase, IInspectorSchemaSource?>(nameof(LocalSchemaSource));

        public static readonly StyledProperty<object?> LocalModelProperty =
            AvaloniaProperty.Register<InspectorItemBase, object?>(nameof(LocalModel));

        public static readonly DirectProperty<InspectorItemBase, IInspectorSchema?> LocalSchemaProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, IInspectorSchema?>(
                nameof(LocalSchema),
                o => o.LocalSchema,
                (o, v) => o.LocalSchema = v);

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

        public static readonly DirectProperty<InspectorItemBase, IInspectorSchema?> ResolvedSchemaProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, IInspectorSchema?>(
                nameof(ResolvedSchema),
                o => o._resolvedSchema,
                (o, v) => o._resolvedSchema = v,
                defaultBindingMode: Avalonia.Data.BindingMode.OneWay);

        public static readonly DirectProperty<InspectorItemBase, object?> ResolvedModelProperty =
            AvaloniaProperty.RegisterDirect<InspectorItemBase, object?>(
                nameof(ResolvedModel),
                o => o._resolvedModel,
                (o, v) => o._resolvedModel = v,
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

        public IInspectorSchemaSource? LocalSchemaSource
        {
            get => GetValue(LocalSchemaSourceProperty);
            set => SetValue(LocalSchemaSourceProperty, value);
        }

        public object? LocalModel
        {
            get => GetValue(LocalModelProperty);
            set => SetValue(LocalModelProperty, value);
        }

        public IInspectorSchema? LocalSchema
        {
            get => _localSchema;
            private set => SetAndRaise(LocalSchemaProperty, ref _localSchema, value);
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

        public IInspectorSchema? ResolvedSchema
        {
            get => _resolvedSchema;
            private set => SetAndRaise(ResolvedSchemaProperty, ref _resolvedSchema, value);
        }

        public object? ResolvedModel
        {
            get => _resolvedModel;
            private set => SetAndRaise(ResolvedModelProperty, ref _resolvedModel, value);
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
            _resolvedSchema = null;
            Property = null;
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (!_visualTreeAttached)
                return;

            if (change.Property == LocalSchemaSourceProperty ||
                change.Property == LocalModelProperty)
            {
                _resolvedSchema = null;
                ApplySchema();
                UpdateSchema();
            }
            else if (change.Property == LocalSchemaProperty ||
                change.Property == PropertyNameProperty)
            {
                UpdateProperty();
            }
        }

        #endregion

        #region Core 

        public virtual void RefreshProperty() { }

        internal protected virtual void OnParentSchemaChanged() { }

        protected virtual void OnSchemaPropertyChanging(PropertyChangingEventArgs e) { }

        protected virtual void OnSchemaPropertyChanged(PropertyChangedEventArgs e) { }

        private void CurrentSchema_PropertyChanging(object? sender, PropertyChangingEventArgs e)
            => OnSchemaPropertyChanging(e);

        private void CurrentSchema_PropertyChanged(object? sender, PropertyChangedEventArgs e)
            => OnSchemaPropertyChanged(e);

        private void ApplySchema()
        {
            if (LocalSchemaSource == null || LocalModel == null)
            {
                LocalSchema = null;
                return;
            }

            LocalSchema = LocalSchemaSource.Resolve(LocalModel);
        }

        // Durchsuche nach Schema und Model in der aktuellen Instanz und den Eltern,
        // bis beides gefunden wurde oder keine Eltern mehr da sind.
        private void UpdateSchema()
        {
            IInspectorSchema? resolvedSchema = LocalSchema;
            object? resolvedModel = LocalModel;

            var parent = Parent;

            while (parent != null)
            {
                if (parent is ContainerViewItem container)
                {
                    resolvedSchema ??= container.Schema;
                    resolvedModel ??= container.Model;
                }

                if (resolvedSchema != null || resolvedModel != null)
                    break;

                parent = parent.Parent;
            }

            ResolvedSchema = resolvedSchema;
            ResolvedModel = resolvedModel;
        }

        protected void UpdateProperty()
        {
            IInspectorPropertyInfo? newProperty = null;

            if (_resolvedSchema != null &&
                PropertyName != null &&
                _resolvedSchema.Properties.TryGetValue(PropertyName, out var propertyInfo))
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