using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Sachssoft.Sasospector.Adapters;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class MultipleValuePropertyEditor : InspectorPropertyEditorBase
    {
        // Aufbau
        // --------
        // 1 Value
        // <Label> [Spinner] <Unit> 

        private const string PART_Switch = nameof(PART_Switch);

        private ToggleSwitch? _partSwitch = null;

        public static readonly StyledProperty<InspectorPropertyAdapterBase<BoundedValue<double>[]>?> AdapterProperty =
            AvaloniaProperty.Register<ItemsControl, InspectorPropertyAdapterBase<BoundedValue<double>[]>?>(nameof(Adapter));

        public static readonly StyledProperty<IEnumerable<object>?> LabelsProperty =
            AvaloniaProperty.Register<ItemsControl, IEnumerable<object>?>(nameof(Labels));

        public static readonly StyledProperty<IEnumerable<NumberUnitSelector>?> UnitsProperty =
            AvaloniaProperty.Register<ItemsControl, IEnumerable<NumberUnitSelector>?>(nameof(Units));

        public static readonly StyledProperty<int?> DecimalPlacesProperty =
            AvaloniaProperty.Register<ItemsControl, int?>(nameof(DecimalPlaces));

        protected override Type StyleKeyOverride { get; } = typeof(BooleanSwitchPropertyEditor);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partSwitch = e.NameScope.Get<ToggleSwitch>(PART_Switch);
        }

        public InspectorPropertyAdapterBase<BoundedValue<double>[]>? Adapter
        {
            get => GetValue(AdapterProperty);
            set => SetValue(AdapterProperty, value);
        }

        public IEnumerable<object>? Labels
        {
            get => GetValue(LabelsProperty);
            set => SetValue(LabelsProperty, value);
        }

        // Wenn Kein = einfach leer und unsichtbar
        // Wenn 1 = Label
        // Wenn mehrere = Dropdown
        public IEnumerable<NumberUnitSelector>? Units
        {
            get => GetValue(UnitsProperty);
            set => SetValue(UnitsProperty, value);
        }

        // null -> unbegrenzt
        public int? DecimalPlaces
        {
            get => GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }
    }
}
