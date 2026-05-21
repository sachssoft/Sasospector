using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.VisualTree;
using Sachssoft.Sasospector.Registries;
using Sachssoft.Sasospector.Views.Editors;
using Sachssoft.Sasospector.Views.Fields;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Sachssoft.Sasospector.Views
{
    [TemplatePart(PART_EditorContent, typeof(ContentControl))]
    [TemplatePart(PART_Container, typeof(ContentControl))]
    public class PropertyViewItem : InspectorItem
    {
        private const string PART_EditorContent = nameof(PART_EditorContent);
        private const string PART_Container = nameof(PART_Container);

        private bool? _propertyFound = null;
        private InspectorControl? _control;
        private InspectorPropertyEditorRegistryBase? _editorRegistry;
        private PropertyEditorBase? _editor;
        private ContentControl? _partEditorContent;
        private ContentControl? _partContainer;
        private bool _commandCanExecute = true;
        private EventHandler? _canExecuteChangeHandler = default;
        private bool _isHeaderCheckable = false;

        public static readonly StyledProperty<Type?> TargetTypeProperty =
            AvaloniaProperty.Register<PropertyViewItem, Type?>(nameof(TargetType));

        public static readonly StyledProperty<string?> PreferredEditorKindProperty =
            AvaloniaProperty.Register<PropertyViewItem, string?>(nameof(PreferredEditorKind));

        public static readonly StyledProperty<PropertyEditorBase?> CustomEditorProperty =
            AvaloniaProperty.Register<PropertyViewItem, PropertyEditorBase?>(nameof(CustomEditor));

        public static readonly DirectProperty<PropertyViewItem, bool> IsHeaderCheckableProperty =
            AvaloniaProperty.RegisterDirect<PropertyViewItem, bool>(
                nameof(IsHeaderCheckable),
                o => o.IsHeaderCheckable);

        public static readonly StyledProperty<bool> IsHeaderCheckedProperty =
            AvaloniaProperty.Register<PropertyViewItem, bool>(nameof(IsHeaderChecked));

        public PropertyViewItem() { }

        protected override Type StyleKeyOverride { get; } = typeof(PropertyViewItem);

        // Optional gewünschter Typ des Eigenschafts 
        // Vorteil: typsicher
        public Type? TargetType
        {
            get => GetValue(TargetTypeProperty);
            set => SetValue(TargetTypeProperty, value);
        }

        // Gewünschte alternative eingebaute Editor Art, wenn VariantEditor = null
        public string? PreferredEditorKind
        {
            get => GetValue(PreferredEditorKindProperty);
            set => SetValue(PreferredEditorKindProperty, value);
        }

        // Gewünschte alternative Editor
        public PropertyEditorBase? CustomEditor
        {
            get => GetValue(CustomEditorProperty);
            set => SetValue(CustomEditorProperty, value);
        }

        public InspectorContainerTemplates ContainerTemplates { get; } = new InspectorContainerTemplates();

        public PropertyEditorActions Actions { get; } = new PropertyEditorActions();

        public bool IsHeaderCheckable
        {
            get => _isHeaderCheckable;
            private set => SetAndRaise(IsHeaderCheckableProperty, ref _isHeaderCheckable, value);
        }

        public bool IsHeaderChecked
        {
            get => GetValue(IsHeaderCheckedProperty);
            set => SetValue(IsHeaderCheckedProperty, value);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            // ✔ einmal sauber im Tree nach oben gehen (Avalonia API statt manueller Parent-Loop)
            _control = this.FindAncestorOfType<InspectorControl>();

            _editorRegistry = _control?.EditorRegistry;
            Build();
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            if (Property != null)
                Property.ValueChanged -= Property_Changed;

            _control = null;
            _editorRegistry = null;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partEditorContent = e.NameScope.Get<ContentControl>(PART_EditorContent);
            _partContainer = e.NameScope.Get<ContentControl>(PART_Container);

            Build();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (/*change.Property == SourceProperty || */
                change.Property == PropertyNameProperty ||
                change.Property == CustomEditorProperty ||
                change.Property == ItemTemplateProperty ||
                change.Property == SchemaProperty)
                Build();

            if (change.Property == SchemaProperty)
            {
            }

            if (change.Property == IsHeaderCheckedProperty)
            {
                //if (_editor is IEditorNullHandling nullHandling)
                //    nullHandling.IsNull = IsHeaderChecked;
            }
        }

        private void Build()
        {
            if (_editorRegistry == null ||
                _partEditorContent == null ||
                Schema == null ||
                string.IsNullOrEmpty(PropertyName))
                return;

            if (_propertyFound == null)
            {
                _propertyFound = Schema.Properties.TryGetValue(PropertyName, out var propertyInfo);

                // TargetTyp = null (nur optional)
                // Gewünschter Zieltyp (Typsicherheit!)
                if (TargetType != null && propertyInfo != null)
                {
                    if (propertyInfo.Type != TargetType)
                    {
                        return;
                    }
                }

                Property = propertyInfo;
            }

            if (Property == null)
                return;

            Property.ValueChanged += Property_Changed;

            // 1. VariantEditor (hard override instance)
            // 2. VariantEditorKind (template selector)
            // 3. Registry (default)

            if (CustomEditor == null)
            {
                var editorRaw = _editorRegistry.Create(Property, PreferredEditorKind);

                if (editorRaw is PropertyEditorBase inspectorPropertyEditor)
                {
                    _editor = inspectorPropertyEditor;
                }
            }
            else
            {
                _editor = CustomEditor;
            }

            if (_editor != null)
            {
                //_editor.Container = this;
                //_editor.FieldHeaders = FieldHeaders?.AsReadOnly();
                //_editor.Source = Property;

                //if (_editor is IItemTemplateProvider itp)
                //{
                //    itp.ItemTemplate = ItemTemplate;
                //}
            }

            _partEditorContent.Content = _editor;

            UpdateContainer();
        }

        private void Property_Changed(object? sender, InspectorPropertyChangedEventArgs e)
        {
            UpdateContainer();
        }

        //private Control? GenerateFieldHeader(FieldHeaderBase? fieldHeader, object? header)
        //{
        //    // 1. explizite Header-Logik (höchste Priorität)
        //    if (fieldHeader != null)
        //    {
        //        var result = fieldHeader.GenerateHeader(header);

        //        if (result != null)
        //            return result;
        //    }

        //    // 2. globales Template
        //    if (FieldHeaderTemplate != null)
        //    {
        //        var templateResult = FieldHeaderTemplate.Build(header);

        //        if (templateResult != null)
        //            return templateResult;
        //    }

        //    // 3. fallback
        //    if (header is Control control)
        //        return control;

        //    return new TextBlock
        //    {
        //        Text = header?.ToString() ?? string.Empty
        //    };
        //}

        private void UpdateContainer()
        {
            //if (_editor == null || _partContainer == null)
            //    return;

            //var value = _editor.Source.GetValue();
            //Debug.WriteLine("Value Changed {0}", value);

            //if (value != null)
            //{
            //    InspectorContainerTemplate? template = null;

            //    // 1. Container-specific
            //    template = ContainerTemplates.FirstOrDefault(t => t.Match(value));

            //    // 2. Global DataTemplates
            //    if (template == null)
            //    {
            //        template = DataTemplates
            //            .OfType<InspectorContainerTemplate>()
            //            .FirstOrDefault(t => t.Match(value));
            //    }

            //    // 3. Resources fallback
            //    if (template == null && Resources != null)
            //    {
            //        template = Resources
            //            .Values
            //            .OfType<InspectorContainerTemplate>()
            //            .FirstOrDefault(t => t.Match(value));
            //    }

            //    if (template != null)
            //    {
            //        var templatedInstance = template.Build(value);
            //        _partContainer.Content = templatedInstance;
            //    }
            //}
        }
    }
}