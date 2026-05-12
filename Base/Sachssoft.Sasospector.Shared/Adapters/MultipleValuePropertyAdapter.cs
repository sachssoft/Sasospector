using Sachssoft.Sasospector.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Sachssoft.Sasospector.Adapters
{
    // Manche andere UI (Spiele) nur Float, andere UI Double
    public class MultipleValuePropertyAdapter<TField, TTarget> : InspectorPropertyAdapterBase<BoundedValue<TField>[]>
        where TField : struct, IMinMaxValue<TField>, INumber<TField>
        where TTarget : struct
    {
        private InspectorPropertyInfo? _propertyInfo;
        private readonly List<Entry> _supportedTypeEntries = new();

        private record Entry(
            Func<InspectorPropertyInfo, bool> CanHandle,
            Func<InspectorPropertyInfo, BoundedValue<TField>[]> Getter,
            Action<InspectorPropertyInfo, BoundedValue<TField>[]> Setter
        );

        public MultipleValuePropertyAdapter(
            Func<TTarget, BoundedValue<TField>[]> toValues,
            Func<TField[], TTarget> fromValues
        )
        {
            RegisterTypes();
        }

        protected override bool OnCanHandle()
        {
            return _propertyInfo != null &&
                   _supportedTypeEntries.Any(x => x.CanHandle(_propertyInfo));
        }

        protected override BoundedValue<TField>[] OnGetValue()
        {
            var entry = GetEntry(_propertyInfo!);
            return entry.Getter(_propertyInfo!);
        }

        protected override void OnSetValue(BoundedValue<TField>[] value)
        {
            var entry = GetEntry(_propertyInfo!);
            entry.Setter(_propertyInfo!, value);
        }

        //protected void RegisterType<TValue>(
        //    Func<InspectorPropertyInfo, bool> canHandle,
        //    Func<TValue, T[]> toValues,
        //    Func<BoundedValue<T>[], TValue> fromValues)
        //    where TValue : struct
        //{
        //    _supportedTypeEntries.Add(new Entry(
        //        canHandle,
        //        pi => TransformTo(pi, toValues),
        //        (pi, values) => TransformBack(pi, values, fromValues)
        //    ));
        //}

        private Entry GetEntry(InspectorPropertyInfo propertyInfo)
        {
            var entry = _supportedTypeEntries
                .FirstOrDefault(x => x.CanHandle(propertyInfo));

            if (entry == null)
                throw new NotSupportedException(
                    $"Type '{propertyInfo.Type}' is not supported.");

            return entry;
        }

        private BoundedValue<TField>[] TransformTo<TValue>(
            InspectorPropertyInfo pi,
            Func<TValue, TField[]> output)
            where TValue : struct
        {
            var value = (TValue?)pi.GetValue() ?? default;

            var values = output(value);

            return GetOutputs(
                pi.Metadata,
                values
            );
        }

        private void TransformBack<TValue>(
            InspectorPropertyInfo pi,
            BoundedValue<TField>[] values,
            Func<BoundedValue<TField>[], TValue> output)
            where TValue : struct
        {
            var result = output(values);
            pi.SetValue(result);
        }

        private BoundedValue<TField>[] GetOutputs(
            InspectorPropertyInfoMetadata metadata,
            IEnumerable<TField> values)
        {
            var (min, max) = GetRange(metadata);

            return values
                .Select(v => new BoundedValue<TField>(v, min, max))
                .ToArray();
        }

        private (TField Min, TField Max) GetRange(
            InspectorPropertyInfoMetadata metadata)
        {
            var constraints = metadata.Constraints;

            if (constraints == null || constraints.Count == 0)
                return (TField.MinValue, TField.MaxValue);

            TField min = TField.MinValue;
            TField max = TField.MaxValue;

            foreach (var c in constraints)
            {
                if (c is RangeConstraint<TField> r)
                {
                    if (r.MinValue > min)
                        min = r.MinValue;

                    if (r.MaxValue < max)
                        max = r.MaxValue;
                }
            }

            if (min > max)
                (min, max) = (max, min);

            return (min, max);
        }

        private void RegisterTypes()
        {
            //RegisterType<byte>(pi => pi.Type == typeof(byte), v => [v], v => (byte)v[0].Value);
            //RegisterType<sbyte>(pi => pi.Type == typeof(sbyte), v => [v], v => (sbyte)v[0].Value);

            //RegisterType<short>(pi => pi.Type == typeof(short), v => [v], v => (short)v[0].Value);
            //RegisterType<ushort>(pi => pi.Type == typeof(ushort), v => [v], v => (ushort)v[0].Value);

            //RegisterType<int>(pi => pi.Type == typeof(int), v => [v], v => (int)v[0].Value);
            //RegisterType<uint>(pi => pi.Type == typeof(uint), v => [v], v => (uint)v[0].Value);

            //RegisterType<long>(pi => pi.Type == typeof(long), v => [v], v => (long)v[0].Value);
            //RegisterType<ulong>(pi => pi.Type == typeof(ulong), v => [v], v => (ulong)v[0].Value);

            //RegisterType<nint>(pi => pi.Type == typeof(nint), v => [v], v => (nint)v[0].Value);
            //RegisterType<nuint>(pi => pi.Type == typeof(nuint), v => [v], v => (nuint)v[0].Value);

            //RegisterType<float>(pi => pi.Type == typeof(float), v => [v], v => (float)v[0].Value);
            //RegisterType<double>(pi => pi.Type == typeof(double), v => [v], v => v[0].Value);
            //RegisterType<decimal>(pi => pi.Type == typeof(decimal), v => [(double)v], v => (decimal)v[0].Value);

            //RegisterType<Half>(pi => pi.Type == typeof(Half), v => [(double)v], v => (Half)v[0].Value);

            //RegisterType<PixelPoint>(pi => pi.Type == typeof(PixelPoint), v => [v.X, v.Y], v => new PixelPoint((int)v[0].Value, (int)v[1].Value));
            //RegisterType<Point>(pi => pi.Type == typeof(Point), v => [v.X, v.Y], v => new Point(v[0].Value, v[1].Value));

            //RegisterType<PixelSize>(pi => pi.Type == typeof(PixelSize), v => [v.Width, v.Height], v => new PixelSize((int)v[0].Value, (int)v[1].Value));
            //RegisterType<Size>(pi => pi.Type == typeof(Size), v => [v.Width, v.Height], v => new Size(v[0].Value, v[1].Value));

            //RegisterType<Vector>(pi => pi.Type == typeof(Vector), v => [v.X, v.Y], v => new Vector(v[0].Value, v[1].Value));
            //RegisterType<Vector3D>(pi => pi.Type == typeof(Vector3D), v => [v.X, v.Y, v.Z], v => new Vector3D(v[0].Value, v[1].Value, v[2].Value));

            //RegisterType<Thickness>(pi => pi.Type == typeof(Thickness), v => [v.Left, v.Top, v.Right, v.Bottom], v => new Thickness(v[0].Value, v[1].Value, v[2].Value, v[3].Value));

            //RegisterType<CornerRadius>(pi => pi.Type == typeof(CornerRadius), v => [v.TopLeft, v.TopRight, v.BottomRight, v.BottomLeft], v => new CornerRadius(v[0].Value, v[1].Value, v[2].Value, v[3].Value));

            //RegisterType<GridLength>(pi => pi.Type == typeof(GridLength), v => [v.Value], v => new GridLength(v[0].Value, GridUnitType.Pixel));
        }
    }
}