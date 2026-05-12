using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.VisualTree;
using Sachssoft.Sasospector.Registries;
using Sachssoft.Sasospector.Views.Editors;
using System;
using System.Reflection;

namespace Sachssoft.Sasospector.Views
{
    [TemplatePart(PART_EditorContent, typeof(ContentControl))]
    public class InspectorPropertyView : InspectorItem
    {
        private const string PART_EditorContent = nameof(PART_EditorContent);

        private bool? _propertyFound = null;
        private IInspectorPropertyInfo? _property;
        private InspectorControl? _control;
        private InspectorPropertyEditorRegistryBase? _editorRegistry;
        private ContentControl? _partEditorContent;

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

            _control = null;
            _editorRegistry = null;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partEditorContent = e.NameScope.Get<ContentControl>(PART_EditorContent);

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

            // 1. VariantEditor (hard override instance)
            // 2. VariantEditorKind (template selector)
            // 3. Registry (default)

            if (CustomEditor == null)
            {
                var editor = _editorRegistry.Create(Property, PreferredEditorKind);

                if (editor is InspectorPropertyEditorBase inspectorPropertyEditor)
                {
                    inspectorPropertyEditor.Source = Property;
                }

                _partEditorContent.Content = editor;
            }
            else
            {
                _partEditorContent.Content = CustomEditor;
            }
        }
    }
}