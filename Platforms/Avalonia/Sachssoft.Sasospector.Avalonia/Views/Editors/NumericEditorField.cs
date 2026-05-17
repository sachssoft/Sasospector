using Avalonia;
using Avalonia.Data;
using Sachssoft.Sasospector.Editors;
using System;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class NumericEditorField : NumericEditorFieldBase
    {
        private bool _kindSyncing;
        private bool _isDataContextChanging;

        public static readonly StyledProperty<double> MinimumProperty =
            AvaloniaProperty.Register<NumericEditorField, double>(nameof(Minimum), coerce: CoerceMinimum);

        public static readonly StyledProperty<double> MaximumProperty =
            AvaloniaProperty.Register<NumericEditorField, double>(nameof(Maximum), 100.0, coerce: CoerceMaximum);

        public static readonly StyledProperty<double> ValueProperty =
            AvaloniaProperty.Register<NumericEditorField, double>(nameof(Value),
                defaultBindingMode: BindingMode.TwoWay,
                coerce: CoerceValue);

        public static readonly StyledProperty<double> SmallChangeProperty =
            AvaloniaProperty.Register<NumericEditorField, double>(nameof(SmallChange), 1.0);

        public static readonly StyledProperty<double> LargeChangeProperty =
            AvaloniaProperty.Register<NumericEditorField, double>(nameof(LargeChange), 10.0);

        public NumericEditorField()
        {
            EditorKindSelector = new EditorKindSelector(
                defaultKind: BooleanEditorKinds.Switch,
                changed: v =>
                {
                    if (_kindSyncing)
                        return;

                    _kindSyncing = true;
                    PreferredKind = v;
                    Build();
                    _kindSyncing = false;
                },
                kinds: new[]
                {
                    MultipleNumericEditorKinds.TextBox,
                    MultipleNumericEditorKinds.Spinner,
                    MultipleNumericEditorKinds.Slider,
                    MultipleNumericEditorKinds.Dropdown
                }
            );
        }

        protected override Type StyleKeyOverride { get; } = typeof(NumericEditorField);

        public override EditorKindSelector EditorKindSelector { get; }

        public double Minimum
        {
            get => GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        private static double CoerceMinimum(AvaloniaObject sender, double value)
        {
            return ValidateDouble(value) ? value : sender.GetValue(MinimumProperty);
        }

        private void OnMinimumChanged()
        {
            if (IsInitialized && !_isDataContextChanging)
            {
                CoerceValue(MaximumProperty);
                CoerceValue(ValueProperty);
            }
        }

        public double Maximum
        {
            get => GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        private static double CoerceMaximum(AvaloniaObject sender, double value)
        {
            return ValidateDouble(value)
                ? Math.Max(value, sender.GetValue(MinimumProperty))
                : sender.GetValue(MaximumProperty);
        }

        private void OnMaximumChanged()
        {
            if (IsInitialized && !_isDataContextChanging)
            {
                CoerceValue(MinimumProperty);
                CoerceValue(ValueProperty);
            }
        }

        public double Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static double CoerceValue(AvaloniaObject sender, double value)
        {
            return ValidateDouble(value)
                ? double.Clamp(value, sender.GetValue(MinimumProperty), sender.GetValue(MaximumProperty))
                : sender.GetValue(ValueProperty);
        }

        public double SmallChange
        {
            get => GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }

        public double LargeChange
        {
            get => GetValue(LargeChangeProperty);
            set => SetValue(LargeChangeProperty, value);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            CoerceValue(MaximumProperty);
            CoerceValue(ValueProperty);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == MinimumProperty)
            {
                OnMinimumChanged();
            }
            else if (change.Property == MaximumProperty)
            {
                OnMaximumChanged();
            }
        }

        protected override void OnDataContextBeginUpdate()
        {
            _isDataContextChanging = true;
            base.OnDataContextBeginUpdate();
        }

        protected override void OnDataContextEndUpdate()
        {
            base.OnDataContextEndUpdate();
            _isDataContextChanging = false;
        }

        private static bool ValidateDouble(double value)
        {
            return !double.IsInfinity(value) && !double.IsNaN(value);
        }

        private void Build()
        {
        }
    }
}