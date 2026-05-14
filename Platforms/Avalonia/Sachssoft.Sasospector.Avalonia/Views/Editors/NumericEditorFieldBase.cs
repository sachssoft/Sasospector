using Avalonia;
using Avalonia.Controls.Primitives;
using Sachssoft.Sasospector.Editors;
using Sachssoft.Sasospector.Registries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Views.Editors
{
    public abstract class NumericEditorFieldBase : TemplatedControl, IMultipleNumericEditorField
    {
        private bool _kindSyncing;
        private bool _sourceSyncing;

        public static readonly StyledProperty<IEnumerable<object>?> HeaderProperty =
            AvaloniaProperty.Register<NumericEditorFieldBase, IEnumerable<object>?>(nameof(Header));

        public static readonly StyledProperty<IEnumerable<NumericUnitSelector>?> UnitsProperty =
            AvaloniaProperty.Register<NumericEditorFieldBase, IEnumerable<NumericUnitSelector>?>(nameof(Units));

        public static readonly StyledProperty<NumericUnitSelector?> SelectedUnitProperty =
            AvaloniaProperty.Register<NumericEditorFieldBase, NumericUnitSelector?>(nameof(SelectedUnit));

        public static readonly StyledProperty<string?> PreferredKindProperty =
            AvaloniaProperty.Register<NumericEditorFieldBase, string?>(nameof(PreferredKind));

        public static readonly StyledProperty<int?> DecimalPlacesProperty =
            AvaloniaProperty.Register<NumericEditorFieldBase, int?>(nameof(DecimalPlaces));

        public object? Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        // Wenn Kein = einfach leer und unsichtbar
        // Wenn 1 = Label
        // Wenn mehrere = Dropdown
        public IEnumerable<NumericUnitSelector>? Units
        {
            get => GetValue(UnitsProperty);
            set => SetValue(UnitsProperty, value);
        }

        public NumericUnitSelector? SelectedUnit
        {
            get => GetValue(SelectedUnitProperty);
            set => SetValue(SelectedUnitProperty, value);
        }

        public abstract EditorKindSelector EditorKindSelector { get; }

        public string? PreferredKind
        {
            get => GetValue(PreferredKindProperty);
            set => SetValue(PreferredKindProperty, value);
        }

        // null -> unbegrenzt
        public int? DecimalPlaces
        {
            get => GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }


        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            //if (change.Property == ValueProperty)
            //{
            //    Debug.WriteLine("Value {0}", Value);
            //}
        }

        private void Build()
        {

        }
    }
}
