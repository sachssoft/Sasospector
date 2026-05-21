using Avalonia;
using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class MultipleNumericEditor : PropertyEditorBase, IMultipleNumericEditor
    {
        private bool _sourceSyncing;
        private bool _decimalMode;
        private bool _allowNull;

        //public static readonly StyledProperty<object[]> ValueProperty =
        //    AvaloniaProperty.Register<MultipleNumericEditor, object[]>(nameof(Value));

        public static readonly StyledProperty<IIndexedPropertyAdapter?> AdapterProperty =
            AvaloniaProperty.Register<MultipleNumericEditor, IIndexedPropertyAdapter?>(nameof(Adapter));

        public static readonly StyledProperty<IReadOnlyList<IMultipleNumericEditorField>?> FieldsProperty =
            AvaloniaProperty.Register<MultipleNumericEditor, IReadOnlyList<IMultipleNumericEditorField>?>(nameof(Fields));

        public static readonly StyledProperty<bool> IsNullProperty =
            AvaloniaProperty.Register<MultipleNumericEditor, bool>(nameof(IsNull));

        protected override Type StyleKeyOverride { get; } = typeof(MultipleNumericEditor);

        //public object[] Value
        //{
        //    get => GetValue(ValueProperty);
        //    set => SetValue(ValueProperty, value);
        //}

        public IIndexedPropertyAdapter? Adapter
        {
            get => GetValue(AdapterProperty);
            set => SetValue(AdapterProperty, value);
        }

        public IReadOnlyList<IMultipleNumericEditorField>? Fields
        {
            get => GetValue(FieldsProperty);
            set => SetValue(FieldsProperty, value);
        }

        public bool AllowNull => _allowNull;

        public bool IsNull
        {
            get => GetValue(IsNullProperty);
            set => SetValue(IsNullProperty, value);
        }

        IMultipleNumericEditorField IMultipleNumericEditor.CreateField(Type valueType)
        {
            if (valueType == typeof(decimal))
            {
                return new DecimalNumericEditorField();
            }
            return new NumericEditorField();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == AdapterProperty || change.Property == PreferredKindProperty)
            {
                if (Adapter != null)
                {
                    //if (Adapter.SupportsField(typeof(decimal)))
                    //{
                    //    DecimalMode = true;
                    //}
                    //else if (Adapter.SupportsField(typeof(double)))
                    //{
                    //    DecimalMode = false;
                    //}
                    //else
                    //{
                    //    // Fehler!!!
                    //}
                }

                Update();
                return;
            }

            if (change.Property == FieldsProperty)
            {
                if (change.OldValue is IReadOnlyList<IMultipleNumericEditorField> oldFields)
                    DetachFields(oldFields);

                SetupFields();
                return;
            }

            //if (change.Property == ValueProperty && !_sourceSyncing)
            //{
            //    PushToSource();
            //}
        }

        protected override void OnPropertySourceValueChanged()
        {
            if (_sourceSyncing || Adapter == null || Source == null)
                return;

            _sourceSyncing = true;

            //var converted = Adapter.ToField(Source.GetValue());

            //if (converted is double[] values)
            //{
            //    Value = values;
            //}

            _sourceSyncing = false;
        }

        private void PushToSource()
        {
            if (Adapter == null || Source == null)
                return;

            _sourceSyncing = true;

            //Source.SetValue(Adapter.ToSource(Value));

            _sourceSyncing = false;
        }

        private void Update()
        {
            if (Fields == null)
                return;

            foreach (var field in Fields.OfType<NumericEditorFieldBase>())
            {
                field.PreferredKind = PreferredKind;
            }
        }

        private void SetupFields()
        {
            if (Fields == null)
                return;

            foreach (var field in Fields.OfType<NumericEditorFieldBase>())
            {
                field.PropertyChanged -= Field_PropertyChanged;
                field.PropertyChanged += Field_PropertyChanged;
            }
        }

        private void DetachFields(IReadOnlyList<IMultipleNumericEditorField> fields)
        {
            foreach (var field in fields.OfType<NumericEditorFieldBase>())
            {
                field.PropertyChanged -= Field_PropertyChanged;
            }
        }

        private void Field_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (!(e.Property == NumericEditorField.ValueProperty ||
                  e.Property == DecimalNumericEditorField.ValueProperty))
                return;

            //if (sender is not NumericEditorFieldBase field)
            //    return;

            //if (Adapter == null || Fields == null)
            //    return;

            //var index = IndexOfField(field);

            //if (index < 0 || index >= Adapter.FieldCount)
            //    return;

            //var values = Value ?? new double[Adapter.FieldCount];

            //if (values.Length != Adapter.FieldCount)
            //    Array.Resize(ref values, Adapter.FieldCount);

            //var copy = (double[])values.Clone();
            //copy[index] = field.Value;

            //Value = copy;
        }

        private int IndexOfField(NumericEditorFieldBase field)
        {
            if (Fields == null)
                return -1;

            for (int i = 0; i < Fields.Count; i++)
            {
                if (ReferenceEquals(Fields[i], field))
                    return i;
            }

            return -1;
        }
    }
}