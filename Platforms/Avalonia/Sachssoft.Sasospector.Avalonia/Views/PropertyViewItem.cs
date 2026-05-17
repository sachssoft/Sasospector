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
    public class PropertyViewItem : InspectorItem, ICommandSource
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
        private bool _commandCanExecute = true;
        private EventHandler? _canExecuteChangeHandler = default;
        private bool _isHeaderCheckable = false;

        public static readonly StyledProperty<IList<FieldHeaderBase>?> FieldHeadersProperty =
            AvaloniaProperty.Register<PropertyViewItem, IList<FieldHeaderBase>?>(nameof(FieldHeaders));

        public static readonly StyledProperty<string?> PropertyNameProperty =
            AvaloniaProperty.Register<PropertyViewItem, string?>(nameof(PropertyName));

        public static readonly StyledProperty<Type?> TargetTypeProperty =
            AvaloniaProperty.Register<PropertyViewItem, Type?>(nameof(TargetType));

        public static readonly DirectProperty<PropertyViewItem, IInspectorPropertyInfo?> PropertyProperty =
            AvaloniaProperty.RegisterDirect<PropertyViewItem, IInspectorPropertyInfo?>(
                nameof(Property),
                o => o.Property);

        public static readonly StyledProperty<string?> PreferredEditorKindProperty =
            AvaloniaProperty.Register<PropertyViewItem, string?>(nameof(PreferredEditorKind));

        public static readonly StyledProperty<InspectorPropertyEditorBase?> CustomEditorProperty =
            AvaloniaProperty.Register<PropertyViewItem, InspectorPropertyEditorBase?>(nameof(CustomEditor));

        public static readonly StyledProperty<IDataTemplate?> FieldHeaderTemplateProperty =
            AvaloniaProperty.Register<PropertyViewItem, IDataTemplate?>(nameof(FieldHeaderTemplate));

        public static readonly StyledProperty<ICommand?> CommandProperty =
            AvaloniaProperty.Register<PropertyViewItem, ICommand?>(nameof(Command));

        public static readonly StyledProperty<object?> CommandParameterProperty =
            AvaloniaProperty.Register<PropertyViewItem, object?>(nameof(CommandParameter));

        public static readonly DirectProperty<PropertyViewItem, bool> IsHeaderCheckableProperty =
            AvaloniaProperty.RegisterDirect<PropertyViewItem, bool>(
                nameof(IsHeaderCheckable),
                o => o.IsHeaderCheckable);

        public static readonly StyledProperty<bool> IsHeaderCheckedProperty =
            AvaloniaProperty.Register<PropertyViewItem, bool>(nameof(IsHeaderChecked));

        public PropertyViewItem() { }

        protected override Type StyleKeyOverride { get; } = typeof(PropertyViewItem);

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

        public string? PropertyName
        {
            get => GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

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

        public ICommand? Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
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

        protected override bool IsEnabledCore => base.IsEnabledCore && _commandCanExecute;

        private EventHandler CanExecuteChangedHandler => _canExecuteChangeHandler ??= new(CanExecuteChanged);

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            // ✔ einmal sauber im Tree nach oben gehen (Avalonia API statt manueller Parent-Loop)
            _control = this.FindAncestorOfType<InspectorControl>();

            _editorRegistry = _control?.EditorRegistry;
            Build();

            (var command, var parameter) = (Command, CommandParameter);
            if (command is not null)
            {
                command.CanExecuteChanged += CanExecuteChangedHandler;
                CanExecuteChanged(command, parameter);
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            if (Command is { } command)
            {
                command.CanExecuteChanged -= CanExecuteChangedHandler;
            }

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

                if (editorRaw is InspectorPropertyEditorBase inspectorPropertyEditor)
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
                _editor.FieldHeaders = FieldHeaders?.AsReadOnly();
                _editor.Source = Property;

                //if (_editor is IEditorNullHandling nullHandling)
                //{
                //    IsHeaderCheckable = nullHandling.AllowNull;
                //    IsHeaderChecked = nullHandling.IsNull;
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

        private void CanExecuteChanged(object? sender, EventArgs e)
        {
            CanExecuteChanged(Command, CommandParameter);
        }

        void ICommandSource.CanExecuteChanged(object sender, EventArgs e)
        {
            CanExecuteChanged(Command, CommandParameter);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void CanExecuteChanged(ICommand? command, object? parameter)
        {
            var canExecute = command == null || command.CanExecute(parameter);

            if (canExecute != _commandCanExecute)
            {
                _commandCanExecute = canExecute;
                UpdateIsEffectivelyEnabled();
            }
        }
    }
}