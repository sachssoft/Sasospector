using Microsoft.VisualBasic.FileIO;
using Sachssoft.Sasospector.Constraints;
using System;
using System.Linq;

namespace Sachssoft.Sasospector.Adapters
{
    // Indexed field access for homogeneous numeric structures
    public sealed class IndexedFieldPropertyAdapter<TSource> : IIndexedPropertyAdapter
    {
        private readonly int _fieldCount;
        private readonly Func<TSource, IRangedValue[]> _castTo;
        private readonly Func<object[], TSource> _castFrom;
        private readonly Type[] _types;
        private readonly object[] _buffer;

        public IndexedFieldPropertyAdapter(
            Type singleFieldType,
            Func<TSource, IRangedValue[]> castTo,
            Func<object[], TSource> castFrom)
            : this([singleFieldType], castTo, castFrom)
        {
        }

        public IndexedFieldPropertyAdapter(
            Type uniformFieldType,
            int fieldCount,
            Func<TSource, IRangedValue[]> castTo,
            Func<object[], TSource> castFrom)
        {
            if (fieldCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(fieldCount));

            if (uniformFieldType == null)
                throw new ArgumentNullException(nameof(uniformFieldType));

            _fieldCount = fieldCount;
            _castTo = castTo ?? throw new ArgumentNullException(nameof(castTo));
            _castFrom = castFrom ?? throw new ArgumentNullException(nameof(castFrom));

            _types = Enumerable.Repeat(uniformFieldType, fieldCount).ToArray();
            _buffer = new object[_fieldCount];
        }

        public IndexedFieldPropertyAdapter(
            Type[] fieldTypes,
            Func<TSource, IRangedValue[]> castTo,
            Func<object[], TSource> castFrom)
        {
            if (fieldTypes == null)
                throw new ArgumentNullException(nameof(fieldTypes));

            if (fieldTypes.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(fieldTypes));

            _fieldCount = fieldTypes.Length;
            _castTo = castTo ?? throw new ArgumentNullException(nameof(castTo));
            _castFrom = castFrom ?? throw new ArgumentNullException(nameof(castFrom));

            _types = fieldTypes;
            _buffer = new object[_fieldCount];
        }

        public int FieldCount => _fieldCount;

        public bool SupportsField(Type targetType) => true;

        public object[] CastTo(TSource sourceValue)
        {
            var boundedValues = _castTo(sourceValue);

            ValidateFieldCount(boundedValues.Length);

            for (int i = 0; i < boundedValues.Length; i++)
            {
                var v = boundedValues[i];

                // UI value only (no generic math here!)
                _buffer[i] = v.Value;
            }

            return _buffer;
        }

        public TSource CastFrom(object[] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            ValidateFieldCount(values.Length);

            return _castFrom(values);
        }

        object? IInspectorPropertyAdapter.ToField(object? sourceValue)
        {
            if (sourceValue is not TSource typed)
                throw new InvalidCastException($"Expected {typeof(TSource).Name}");

            return CastTo(typed);
        }

        object? IInspectorPropertyAdapter.ToSource(object? adapterValue)
        {
            if (adapterValue is not object[] typed)
                throw new InvalidCastException("Expected object[]");

            return CastFrom(typed);
        }

        public Type GetValueType(int fieldIndex)
        {
            if ((uint)fieldIndex >= (uint)_fieldCount)
                throw new ArgumentOutOfRangeException(nameof(fieldIndex));

            return _types[fieldIndex];
        }

        public object? GetValue(int fieldIndex)
        {
            if ((uint)fieldIndex >= (uint)_fieldCount)
                throw new ArgumentOutOfRangeException(nameof(fieldIndex));

            return _buffer[fieldIndex];
        }

        public void SetValue(int fieldIndex, object? value)
        {
            if ((uint)fieldIndex >= (uint)_fieldCount)
                throw new ArgumentOutOfRangeException(nameof(fieldIndex));

            _buffer[fieldIndex] = value;
        }

        private void ValidateFieldCount(int actual)
        {
            if (actual != _fieldCount)
                throw new ArgumentOutOfRangeException(
                    nameof(actual),
                    $"Expected {_fieldCount} fields.");
        }
    }
}