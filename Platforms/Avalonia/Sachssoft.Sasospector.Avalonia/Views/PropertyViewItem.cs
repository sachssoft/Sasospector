using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using Sachssoft.Sasospector.Registries;
using Sachssoft.Sasospector.Views.Editors;
using System;

namespace Sachssoft.Sasospector.Views
{
    [TemplatePart(PART_EditorContent, typeof(ContentControl))]
    [TemplatePart(PART_Container, typeof(ContentControl))]
    public class PropertyViewItem : InspectorItemBase
    {
        private const string PART_EditorContent = nameof(PART_EditorContent);
        private const string PART_Container = nameof(PART_Container);

        private bool? _propertyFound;
        private InspectorControl? _control;
        private InspectorPropertyEditorRegistryBase? _editorRegistry;
        private PropertyEditorBase? _editor;
        private ContentControl? _partEditorContent;
        private ContentControl? _partContainer;

        private bool _isHeaderCheckable;

        public static readonly StyledProperty<Type?> TargetTypeProperty =
            AvaloniaProperty.Register<PropertyViewItem, Type?>(nameof(TargetType));

        public static readonly StyledProperty<string?> PreferredEditorKindProperty =
            AvaloniaProperty.Register<PropertyViewItem, string?>(nameof(PreferredEditorKind));

        public static readonly StyledProperty<PropertyEditorBase?> CustomEditorProperty =
            AvaloniaProperty.Register<PropertyViewItem, PropertyEditorBase?>(nameof(CustomEditor));

        public static readonly DirectProperty<PropertyViewItem, bool> IsHeaderCheckableProperty =
            AvaloniaProperty.RegisterDirect<PropertyViewItem, bool>(
                nameof(IsHeaderCheckable),
                o => o.IsHeaderCheckable,
                (o, v) => o.IsHeaderCheckable = v);

        public static readonly StyledProperty<bool> IsHeaderCheckedProperty =
            AvaloniaProperty.Register<PropertyViewItem, bool>(nameof(IsHeaderChecked));

        public Type? TargetType
        {
            get => GetValue(TargetTypeProperty);
            set => SetValue(TargetTypeProperty, value);
        }

        public string? PreferredEditorKind
        {
            get => GetValue(PreferredEditorKindProperty);
            set => SetValue(PreferredEditorKindProperty, value);
        }

        public PropertyEditorBase? CustomEditor
        {
            get => GetValue(CustomEditorProperty);
            set => SetValue(CustomEditorProperty, value);
        }

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

        public InspectorContainerTemplates ContainerTemplates { get; } = new InspectorContainerTemplates();
        public PropertyEditorActions Actions { get; } = new PropertyEditorActions();

        protected override Type StyleKeyOverride => typeof(PropertyViewItem);

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            _control = this.FindAncestorOfType<InspectorControl>();
            _editorRegistry = _control?.EditorRegistry;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _control = null;
            _editorRegistry = null;
            _editor = null;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partEditorContent = e.NameScope.Get<ContentControl>(PART_EditorContent);
            _partContainer = e.NameScope.Get<ContentControl>(PART_Container);

            UpdateProperty();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == CustomEditorProperty ||
                change.Property == ItemTemplateProperty)
            {
                UpdateProperty();
            }
        }

        protected override IInspectorPropertyInfo? PreferredPropertyOverride(IInspectorPropertyInfo? basePropertyInfo)
        {
            if (TargetType != null && basePropertyInfo != null)
            {
                if (basePropertyInfo.Type != TargetType)
                    return null;
            }

            return basePropertyInfo;
        }

        protected override void OnUpdatePropertyEnter(IInspectorPropertyInfo propertyInfo)
        {
            if (_editorRegistry == null || _partEditorContent == null)
                return;

            _editor = null;
            _partEditorContent.Content = null;

            if (CustomEditor == null)
            {
                var editorRaw = _editorRegistry.Create(propertyInfo, PreferredEditorKind);

                if (editorRaw is PropertyEditorBase editor)
                    _editor = editor;
            }
            else
            {
                _editor = CustomEditor;
            }

            _partEditorContent.Content = _editor;
        }
    }
}