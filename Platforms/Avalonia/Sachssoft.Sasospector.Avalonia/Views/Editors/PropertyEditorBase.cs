using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Views.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Input;
using System.Xml.Linq;

namespace Sachssoft.Sasospector.Views.Editors
{
    public abstract class PropertyEditorBase : TemplatedControl, IPropertyEditor
    {
        private CultureInfo _effectiveCulture = CultureInfo.CurrentUICulture;
        private IInspectorPropertyInfo? _source;
        private IReadOnlyList<FieldHeaderBase>? _fieldHeaders;
        private InspectorItemBase? _container;

        public static readonly DirectProperty<PropertyEditorBase, InspectorItemBase?> ContainerProperty =
            AvaloniaProperty.RegisterDirect<PropertyEditorBase, InspectorItemBase?>(
                nameof(Container),
                o => o.Container,
                (o, v) => o.Container = v);

        public static readonly StyledProperty<bool> IsHeaderVisibleProperty =
            AvaloniaProperty.Register<PropertyEditorBase, bool>(nameof(IsHeaderVisible), defaultValue: true);

        public static readonly StyledProperty<CultureInfo?> CultureProperty =
            AvaloniaProperty.Register<PropertyEditorBase, CultureInfo?>(nameof(Culture));

        public static readonly StyledProperty<ICommand?> BrowseCommandProperty =
            AvaloniaProperty.Register<PropertyEditorBase, ICommand?>(nameof(BrowseCommand));

        public static readonly StyledProperty<string?> PreferredKindProperty =
            AvaloniaProperty.Register<PropertyEditorBase, string?>(nameof(PreferredKind));

        public static readonly DirectProperty<PropertyEditorBase, IInspectorPropertyInfo?> SourceProperty =
            AvaloniaProperty.RegisterDirect<PropertyEditorBase, IInspectorPropertyInfo?>(
                nameof(Source),
                o => o.Source,
                (o, v) => o.Source = v);

        public static readonly DirectProperty<PropertyEditorBase, IReadOnlyList<FieldHeaderBase>?> FieldHeadersProperty =
            AvaloniaProperty.RegisterDirect<PropertyEditorBase, IReadOnlyList<FieldHeaderBase>?>(
                nameof(Source),
                o => o.FieldHeaders,
                (o, v) => o.FieldHeaders = v);

        // null = CurrentCultureUI
        public CultureInfo? Culture
        {
            get => GetValue(CultureProperty);
            set => SetValue(CultureProperty, value);
        }

        public ICommand? BrowseCommand
        {
            get => GetValue(BrowseCommandProperty);
            set => SetValue(BrowseCommandProperty, value);
        }

        // Null ist standardiert, mit Wert alternative Steuerung sofern vorhanden
        // Z.B. BooleanEditor: Switch ist Standard, Alternativ auch CheckBox möglich
        public string? PreferredKind
        {
            get => GetValue(PreferredKindProperty);
            set => SetValue(PreferredKindProperty, value);
        }

        public CultureInfo EffectiveCulture => _effectiveCulture;

        public IInspectorPropertyInfo? Source
        {
            get => _source!;
            internal set
            {
                SetAndRaise(SourceProperty, ref _source, value);

                if (_source != null)
                {
                    _source.ValueChanged += SourceValueChanged;
                }
            }
        }

        public IReadOnlyList<FieldHeaderBase>? FieldHeaders
        {
            get => _fieldHeaders;
            internal set => SetAndRaise(FieldHeadersProperty, ref _fieldHeaders, value);
        }

        public InspectorItemBase? Container
        {
            get => _container;
            protected set => SetAndRaise(ContainerProperty, ref _container, value);
        }

        public bool IsHeaderVisible
        {
            get => GetValue(IsHeaderVisibleProperty);
            set => SetValue(IsHeaderVisibleProperty, value);
        }

        protected void SetSelectedFieldValue(object? value)
        {
            if (Container != null)
                Container.SelectedFieldValue = value;
        }

        protected bool TryMatchFieldHeader(int index, Type? dataType, object? dataValue, [MaybeNullWhen(false)] out FieldHeaderBase header)
        {
            header = null;

            if (FieldHeaders == null)
                return false;

            foreach (var field in FieldHeaders)
            {
                if (field.Match(index, dataType, dataValue))
                {
                    header = field;
                    return true;
                }
            }

            return false;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            if (Container == null)
            {
                var parent = Parent;
                while (parent != null)
                {
                    if (parent is InspectorItemBase item)
                    {
                        Container = item;
                        Source = item.Property;
                        FieldHeaders = item.FieldHeaders?.AsReadOnly();

                        if (this is IItemTemplateProvider itp)
                        {
                            itp.ItemTemplate = item.ItemTemplate;
                        }

                        OnContainerEnter();
                        break;
                    }
                    parent = parent.Parent;
                }
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            if (Container != null)
            {
                OnContainerExit();

                if (this is IItemTemplateProvider itp)
                {
                    itp.ItemTemplate = null;
                }

                FieldHeaders = null;
                Source = null;
                Container = null;
            }
        }

        protected virtual void OnContainerEnter() { }

        protected virtual void OnContainerExit() { }

        protected virtual void OnPropertySourceValueChanged()
        {
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == CultureProperty)
            {
                _effectiveCulture = Culture ?? CultureInfo.CurrentUICulture;
            }
        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {
            if (_source != null)
            {
                _source.ValueChanged -= SourceValueChanged;
            }

            base.OnUnloaded(e);
        }

        private void SourceValueChanged(object? sender, InspectorPropertyChangedEventArgs e)
        {
#if DEBUG
            Debug.WriteLine(e.Property.ToString());
#endif

            OnPropertySourceValueChanged();
        }
    }
}
