using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Sachssoft.Sasospector.Registries;
using Sachssoft.Sasospector.Views.Editors;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Sachssoft.Sasospector.Views
{
    [TemplatePart(PART_EditorContent, typeof(ContentControl))]
    [TemplatePart(PART_Container, typeof(ContentControl))]
    public class InspectorPropertyView : InspectorItem
    {
        private const string PART_EditorContent = nameof(PART_EditorContent);
        private const string PART_Container = nameof(PART_Container);

        private bool? _propertyFound = null;
        private IInspectorPropertyInfo? _property;
        private InspectorControl? _control;
        private InspectorPropertyEditorRegistryBase? _editorRegistry;
        private InspectorPropertyEditorBase? _editor;
        private ContentControl? _partEditorContent;
        private ContentControl? _partContainer;

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<InspectorPropertyView, string?>(nameof(PropertyName));

        public static readonly DirectProperty<InspectorPropertyView, IInspectorPropertyInfo?> PropertyProperty =
            AvaloniaProperty.RegisterDirect<InspectorPropertyView, IInspectorPropertyInfo?>(
                nameof(Property),
                o => o.Property);

        public static readonly StyledProperty<string?> PreferredEditorKindProperty =
            AvaloniaProperty.Register<InspectorPropertyView, string?>(nameof(PreferredEditorKind));

        public static readonly StyledProperty<InspectorPropertyEditorBase?> CustomEditorProperty =
            AvaloniaProperty.Register<InspectorPropertyView, InspectorPropertyEditorBase?>(nameof(CustomEditor));

        public InspectorPropertyView() { }

        protected override Type StyleKeyOverride { get; } = typeof(InspectorPropertyView);

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        // Gewünschte alternative eingebaute Editor Art, wenn VariantEditor = null
        public string? PreferredEditorKind
        {
            get => GetValue(PreferredEditorKindProperty);
            set => SetValue(PreferredEditorKindProperty, value);
        }

        // Gewünschte alternative Editor
        public InspectorPropertyEditorBase? CustomEditor
        {
            get => GetValue(CustomEditorProperty);
            set => SetValue(CustomEditorProperty, value);
        }

        public InspectorContainerTemplates ContainerTemplates { get; } = new InspectorContainerTemplates();

        // Nur ReadOnly wichtig für Bindings
        public IInspectorPropertyInfo? Property
        {
            get => _property;
            private set => SetAndRaise(PropertyProperty, ref _property, value);
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
                change.Property == SchemaProperty)
                Build();

            if (change.Property == SchemaProperty)
            {
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

                if (editorRaw is InspectorPropertyEditorBase inspectorPropertyEditor)
                {
                    _editor = inspectorPropertyEditor;
                    _editor.Source = Property;
                }
            }
            else
            {
                _editor = CustomEditor;
                _editor.Source = Property;
            }

            _partEditorContent.Content = _editor;

            UpdateContainer();
        }

        private void Property_Changed(object? sender, InspectorPropertyChangedEventArgs e)
        {
            UpdateContainer();
        }

        private void UpdateContainer()
        {
            if (_editor == null || _partContainer == null)
                return;

            var value = _editor.Source.GetValue();
            Debug.WriteLine("Value Changed {0}", value);

            if (value != null)
            {
                InspectorContainerTemplate? template = null;

                // 1. Container-specific
                template = ContainerTemplates.FirstOrDefault(t => t.Match(value));

                // 2. Global DataTemplates
                if (template == null)
                {
                    template = DataTemplates
                        .OfType<InspectorContainerTemplate>()
                        .FirstOrDefault(t => t.Match(value));
                }

                // 3. Resources fallback
                if (template == null && Resources != null)
                {
                    template = Resources
                        .Values
                        .OfType<InspectorContainerTemplate>()
                        .FirstOrDefault(t => t.Match(value));
                }

                if (template != null)
                {
                    _partContainer.Content = template.Build(value);
                }
            }
        }
    }
}