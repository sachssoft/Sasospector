using Avalonia;
using Avalonia.Data;
using Sachssoft.Sasospector.Editors;
using System;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class DecimalNumericEditorField : NumericEditorFieldBase
    {
        private bool _kindSyncing;
        private bool _isDataContextChanging;

        public static readonly StyledProperty<decimal> MinimumProperty =
            AvaloniaProperty.Register<DecimalNumericEditorField, decimal>(nameof(Minimum), coerce: CoerceMinimum);

        public static readonly StyledProperty<decimal> MaximumProperty =
            AvaloniaProperty.Register<DecimalNumericEditorField, decimal>(nameof(Maximum), 100.0m, coerce: CoerceMaximum);

        public static readonly StyledProperty<decimal> ValueProperty =
            AvaloniaProperty.Register<DecimalNumericEditorField, decimal>(nameof(Value),
                defaultBindingMode: BindingMode.TwoWay,
                coerce: CoerceValue);

        public static readonly StyledProperty<decimal> SmallChangeProperty =
            AvaloniaProperty.Register<DecimalNumericEditorField, decimal>(nameof(SmallChange), 0.1m);

        public static readonly StyledProperty<decimal> LargeChangeProperty =
            AvaloniaProperty.Register<DecimalNumericEditorField, decimal>(nameof(LargeChange), 1.0m);

        public DecimalNumericEditorField()
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

        protected override Type StyleKeyOverride { get; } = typeof(DecimalNumericEditorField);

        public override EditorKindSelector EditorKindSelector { get; }

        public decimal Minimum
        {
            get => GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        private static decimal CoerceMinimum(AvaloniaObject sender, decimal value)
        {
            var control = (DecimalNumericEditorField)sender;
            return Math.Min(value, control.Maximum);
        }

        private void OnMinimumChanged()
        {
            if (IsInitialized && !_isDataContextChanging)
            {
                CoerceValue(MaximumProperty);
                CoerceValue(ValueProperty);
            }
        }

        public decimal Maximum
        {
            get => GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        private static decimal CoerceMaximum(AvaloniaObject sender, decimal value)
        {
            var control = (DecimalNumericEditorField)sender;
            return Math.Max(value, control.Minimum);
        }

        private void OnMaximumChanged()
        {
            if (IsInitialized && !_isDataContextChanging)
            {
                CoerceValue(MinimumProperty);
                CoerceValue(ValueProperty);
            }
        }

        public decimal Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static decimal CoerceValue(AvaloniaObject sender, decimal value)
        {
            var control = (DecimalNumericEditorField)sender;
            return Math.Min(control.Maximum, Math.Max(control.Minimum, value));
        }

        public decimal SmallChange
        {
            get => GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }

        public decimal LargeChange
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

        private void Build()
        {
        }

    }
}