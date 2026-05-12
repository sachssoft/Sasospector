using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Sachssoft.Sasospector.Registries;
using System.Globalization;
using System.Windows.Input;

namespace Sachssoft.Sasospector.Views.Editors
{
    public abstract class InspectorPropertyEditorBase : TemplatedControl, IPropertyEditor
    {
        private CultureInfo _effectiveCulture = CultureInfo.CurrentUICulture;
        private IInspectorPropertyInfo? _source;

        public static readonly StyledProperty<CultureInfo?> CultureProperty =
            AvaloniaProperty.Register<InspectorPropertyEditorBase, CultureInfo?>(nameof(Culture));

        public static readonly StyledProperty<ICommand?> BrowseCommandProperty =
            AvaloniaProperty.Register<InspectorPropertyEditorBase, ICommand?>(nameof(BrowseCommand));

        public static readonly StyledProperty<string?> PreferredKindProperty =
            AvaloniaProperty.Register<InspectorPropertyEditorBase, string?>(nameof(PreferredKind));

        public static readonly DirectProperty<InspectorPropertyEditorBase, IInspectorPropertyInfo?> SourceProperty =
            AvaloniaProperty.RegisterDirect<InspectorPropertyEditorBase, IInspectorPropertyInfo?>(
                nameof(Source),
                o => o.Source,
                (o, v) => o.Source = v);

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

        public IInspectorPropertyInfo Source
        {
            get => _source!;
            internal set
            {
                SetAndRaise(SourceProperty, ref _source, value);
                
                if (_source != null)
                {
                    _source.Changed += SourceChanged;
                }
            }
        }

        protected virtual void OnPropertySourceChanged() { }

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
                _source.Changed -= SourceChanged;
            }

            base.OnUnloaded(e);
        }

        private void SourceChanged(object? sender, InspectorPropertyChangedEventArgs e)
        {
            OnPropertySourceChanged();
        }
    }
}
