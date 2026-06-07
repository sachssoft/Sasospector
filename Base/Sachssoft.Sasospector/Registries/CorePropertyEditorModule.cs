using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Editors;
using Sachssoft.Sasospector.Purposes;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sachssoft.Sasospector.Registries
{
    public sealed class CorePropertyEditorModule : IInspectorPropertyEditorModule
    {
        public const int HALF_DECIMAL_PLACES = 3;
        public const int FLOAT_DECIMAL_PLACES = 6;

        public void Register(InspectorPropertyEditorRegistryBase registry)
        {
            RegisterPrimitiveTypes(registry);
            RegisterPlatformDependencyTypes(registry);
            RegisterFileSystemTypes(registry);
            RegisterCommonTypes(registry);
            RegisterDataTimeTypes(registry);
            RegisterNumericMathTypes(registry);
            RegisterFeatureTypes(registry);
            RegisterRules(registry);
        }

        private void RegisterFeatureTypes(InspectorPropertyEditorRegistryBase registry)
        {

            // Unterstützt auch Command Selection
            registry.RegisterRule(
                type => IsEnumerableOfDelegate(type),
                (pi, f) => f.CreateDelegateSelector());
        }

        private static bool IsEnumerableOfDelegate(Type t)
        {
            if (!t.IsGenericType)
                return false;

            if (t.GetGenericTypeDefinition() != typeof(IEnumerable<>))
                return false;

            Type itemType = t.GetGenericArguments()[0];

            var result = typeof(Delegate).IsAssignableFrom(itemType);
            return result;
        }

        private void RegisterRules(InspectorPropertyEditorRegistryBase registry)
        {
            // Enum
            registry.RegisterRule(
                type => type.IsEnum && !type.IsDefined(typeof(FlagsAttribute), false),
                (pi, f) => f.CreateEnumEditor(selectionMode: EnumSelectionMode.Single));

            registry.RegisterRule(
                type => type.IsEnum && type.IsDefined(typeof(FlagsAttribute), false),
                (pi, f) => f.CreateEnumEditor(selectionMode: EnumSelectionMode.Multiple));

            // NULLABLE
            registry.RegisterRedirection(
                match: type => Nullable.GetUnderlyingType(type) != null,
                target: type => Nullable.GetUnderlyingType(type)!);

            //// ARRAY (optional)
            //RegisterRule(
            //    type => type.IsArray,
            //    info => new ArrayPropertyEditor(),
            //    priority: 800);

            registry.RegisterRule(
                type =>
                    typeof(System.Collections.IList).IsAssignableFrom(type) ||
                    typeof(System.Collections.Generic.IList<>).IsAssignableFrom(type),
                (pi, f) => f.CreateListEditor());

            registry.RegisterRule(
                match: type =>
                    type != typeof(string) &&
                    !type.IsValueType &&
                    !type.IsArray &&
                    !typeof(System.Collections.IEnumerable).IsAssignableFrom(type),
                factory: (pi, f) => f.CreateInstanceSelector(
                        allowNullSelection: true
                    ));
        }

        private void RegisterPrimitiveTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(bool),
                (f) => f.CreateEditor(typeof(IBooleanEditor)),
                isFallback: true);

            registry.RegisterType(typeof(sbyte),
                (f) => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<sbyte>(
                        singleFieldType: typeof(sbyte),
                        castTo: x => [new BoundedValue<sbyte>(x, sbyte.MinValue, sbyte.MaxValue)],
                        castFrom: y => (sbyte)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(short),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<short>(
                        singleFieldType: typeof(short),
                        x => [new BoundedValue<short>(x, short.MinValue, short.MaxValue)],
                        y => (short)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(int),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<int>(
                        singleFieldType: typeof(int),
                        x => [new BoundedValue<int>(x, int.MinValue, int.MaxValue)],
                        y => (int)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(long),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<long>(
                        singleFieldType: typeof(long),
                        x => [new BoundedValue<long>(x, long.MinValue, long.MaxValue)],
                        y => (long)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(Int128),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<Int128>(
                        singleFieldType: typeof(Int128),
                        x => [new BoundedValue<Int128>(x, Int128.MinValue, Int128.MaxValue)],
                        y => (Int128)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(byte),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<byte>(
                        singleFieldType: typeof(byte),
                        x => [new BoundedValue<byte>(x, byte.MinValue, byte.MaxValue)],
                        y => (byte)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(ushort),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<ushort>(
                        singleFieldType: typeof(ushort),
                        x => [new BoundedValue<ushort>(x, ushort.MinValue, ushort.MaxValue)],
                        y => (ushort)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(uint),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<uint>(
                        singleFieldType: typeof(uint),
                        x => [new BoundedValue<uint>(x, uint.MinValue, uint.MaxValue)],
                        y => (uint)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(ulong),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<ulong>(
                        singleFieldType: typeof(ulong),
                        x => [new BoundedValue<ulong>(x, ulong.MinValue, ulong.MaxValue)],
                        y => (ulong)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(Half),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: HALF_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<Half>(
                        singleFieldType: typeof(Half),
                        x => [new BoundedValue<Half>(x, Half.MinValue, Half.MaxValue)],
                        y => (Half)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(float),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<float>(
                        singleFieldType: typeof(float),
                        x => [new BoundedValue<float>(x, float.MinValue, float.MaxValue)],
                        y => (float)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(double),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<double>(
                        singleFieldType: typeof(double),
                        x => [new BoundedValue<double>(x, double.MinValue, double.MaxValue)],
                        y => (double)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(decimal),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<decimal>(
                        singleFieldType: typeof(decimal),
                        x => [new BoundedValue<decimal>(x, decimal.MinValue, decimal.MaxValue)],
                        y => (decimal)y[0]
                    )
                ),
                isFallback: true);
        }

        private void RegisterPlatformDependencyTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(nint),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<nint>(
                        singleFieldType: typeof(nint),
                        x => [new BoundedValue<nint>(x, nint.MinValue, nint.MaxValue)],
                        y => (nint)y[0]
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(nuint),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<nuint>(
                        singleFieldType: typeof(nuint),
                        x => [new BoundedValue<nuint>(x, nuint.MinValue, nuint.MaxValue)],
                        y => (nuint)y[0]
                    )
                ),
                isFallback: true);
        }

        private void RegisterFileSystemTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(FileInfo),
                (f) => f.CreateFileSystemEditor(
                    mode: FileSystemMode.File
                ),
                isFallback: true);

            registry.RegisterType(typeof(DirectoryInfo),
                (f) => f.CreateFileSystemEditor(
                    mode: FileSystemMode.Directory
                ),
                isFallback: true);

            registry.RegisterType(typeof(string),
                (f) => f.CreateFileSystemEditor(
                    mode: FileSystemMode.File
                ),
                purpose: new FileSystemPurpose
                {
                    Mode = FileSystemMode.File
                });

            registry.RegisterType(typeof(string),
                (f) => f.CreateFileSystemEditor(
                    mode: FileSystemMode.Directory
                ),
                purpose: new FileSystemPurpose
                {
                    Mode = FileSystemMode.Directory
                });
        }

        private void RegisterCommonTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(string),
                (f) => f.CreateUriEditor(),
                purpose: new UriPurpose());

            registry.RegisterType(typeof(string),
                (f) => f.CreateStringEditor(),
                isFallback: true);

            registry.RegisterType(typeof(Uri),
                (f) => f.CreateUriEditor(),
                isFallback: true);

            registry.RegisterType(typeof(char),
                (f) => f.CreateStringEditor(),
                isFallback: true);

            registry.RegisterType(typeof(Version),
                (f) => f.CreateVersionEditor(),
                isFallback: true);

            registry.RegisterType(typeof(Guid),
                (f) => f.CreateGuidEditor(),
                isFallback: true);
        }

        private void RegisterDataTimeTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(DateTime),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.DateTime,
                    adapter: null
                ),
                isFallback: true);

            registry.RegisterType(typeof(DateTimeOffset),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.DateTime,
                    adapter: null
                ),
                isFallback: true);

            registry.RegisterType(typeof(TimeSpan),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.Time,
                    adapter: null
                ),
                isFallback: true);

            registry.RegisterType(typeof(TimeOnly),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.Time,
                    adapter: null
                ),
                isFallback: true);

            registry.RegisterType(typeof(DateOnly),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.Date,
                    adapter: null
                ),
                isFallback: true);
        }

        private void RegisterNumericMathTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(System.Numerics.Vector2),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.Vector2>(
                        uniformFieldType: typeof(float),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<float>(x.X, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Y, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new System.Numerics.Vector2(
                                            x: (float)y[0],
                                            y: (float)y[1]
                                         )
                    ),
                    fieldNames: [
                              new(nameof(System.Numerics.Vector2.X)),
                              new(nameof(System.Numerics.Vector2.Y))
                            ]
                ),
                isFallback: true);

            registry.RegisterType(typeof(System.Numerics.Vector3),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.Vector3>(
                        uniformFieldType: typeof(float),
                        fieldCount: 3,
                        castTo: x => [
                                         new BoundedValue<float>(x.X, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.Y, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.Z, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new System.Numerics.Vector3(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2]
                                         )
                    ),
                    fieldNames: [
                              new(nameof(System.Numerics.Vector3.X)),
                              new(nameof(System.Numerics.Vector3.Y)),
                              new(nameof(System.Numerics.Vector3.Z))
                            ]
                ),
                isFallback: true);

            registry.RegisterType(typeof(System.Numerics.Vector4),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.Vector4>(
                        uniformFieldType: typeof(float),
                        fieldCount: 4,
                        castTo: x => [
                                         new BoundedValue<float>(x.X, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.Y, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.Z, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.W, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new System.Numerics.Vector4(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2],
                                            w: (float)y[3]
                                         )
                    ),
                    fieldNames: [
                              new(nameof(System.Numerics.Vector4.X)),
                              new(nameof(System.Numerics.Vector4.Y)),
                              new(nameof(System.Numerics.Vector4.Z)),
                              new(nameof(System.Numerics.Vector4.W))
                            ]
                ),
                isFallback: true);

            registry.RegisterType(typeof(System.Numerics.Matrix3x2),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.Matrix3x2>(
                        uniformFieldType: typeof(float),
                        fieldCount: 6,
                        castTo: x => [
                                         new BoundedValue<float>(x.M11, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M12, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M21, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M22, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M31, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M32, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new System.Numerics.Matrix3x2(
                                            m11: (float)y[0],
                                            m12: (float)y[1],
                                            m21: (float)y[2],
                                            m22: (float)y[3],
                                            m31: (float)y[4],
                                            m32: (float)y[5]
                                         )
                    ),
                    fieldNames: [
                              new(nameof(System.Numerics.Matrix3x2.M11)),
                              new(nameof(System.Numerics.Matrix3x2.M12)),
                              new(nameof(System.Numerics.Matrix3x2.M21)),
                              new(nameof(System.Numerics.Matrix3x2.M22)),
                              new(nameof(System.Numerics.Matrix3x2.M31)),
                              new(nameof(System.Numerics.Matrix3x2.M32))
                            ]
                ),
                isFallback: true);

            registry.RegisterType(typeof(System.Numerics.Matrix4x4),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.Matrix4x4>(
                        uniformFieldType: typeof(float),
                        fieldCount: 16,
                        castTo: x => [
                                         new BoundedValue<float>(x.M11, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M12, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M13, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M14, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M21, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M22, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M23, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M24, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M31, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M32, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M33, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M34, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M41, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M42, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M43, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.M44, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new System.Numerics.Matrix4x4(
                                            m11: (float)y[0],
                                            m12: (float)y[1],
                                            m13: (float)y[2],
                                            m14: (float)y[3],
                                            m21: (float)y[4],
                                            m22: (float)y[5],
                                            m23: (float)y[6],
                                            m24: (float)y[7],
                                            m31: (float)y[8],
                                            m32: (float)y[9],
                                            m33: (float)y[10],
                                            m34: (float)y[11],
                                            m41: (float)y[12],
                                            m42: (float)y[13],
                                            m43: (float)y[14],
                                            m44: (float)y[15]
                                         )
                    ),
                    fieldNames: [
                              new(nameof(System.Numerics.Matrix4x4.M11)),
                              new(nameof(System.Numerics.Matrix4x4.M12)),
                              new(nameof(System.Numerics.Matrix4x4.M13)),
                              new(nameof(System.Numerics.Matrix4x4.M14)),
                              new(nameof(System.Numerics.Matrix4x4.M21)),
                              new(nameof(System.Numerics.Matrix4x4.M22)),
                              new(nameof(System.Numerics.Matrix4x4.M23)),
                              new(nameof(System.Numerics.Matrix4x4.M24)),
                              new(nameof(System.Numerics.Matrix4x4.M31)),
                              new(nameof(System.Numerics.Matrix4x4.M32)),
                              new(nameof(System.Numerics.Matrix4x4.M33)),
                              new(nameof(System.Numerics.Matrix4x4.M34)),
                              new(nameof(System.Numerics.Matrix4x4.M41)),
                              new(nameof(System.Numerics.Matrix4x4.M42)),
                              new(nameof(System.Numerics.Matrix4x4.M43)),
                              new(nameof(System.Numerics.Matrix4x4.M44))
                            ]
                ),
                isFallback: true);

            registry.RegisterType(typeof(System.Numerics.Quaternion),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.Quaternion>(
                        uniformFieldType: typeof(float),
                        fieldCount: 4,
                        castTo: x => [
                                         new BoundedValue<float>(x.X, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.Y, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.Z, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.W, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new System.Numerics.Quaternion(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2],
                                            w: (float)y[3]
                                         )
                    ),
                    fieldNames: [
                              new(nameof(System.Numerics.Quaternion.X)),
                              new(nameof(System.Numerics.Quaternion.Y)),
                              new(nameof(System.Numerics.Quaternion.Z)),
                              new(nameof(System.Numerics.Quaternion.W))
                            ]
                ),
                isFallback: true);

            registry.RegisterType(typeof(System.Numerics.Plane),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.Plane>(
                        uniformFieldType: typeof(float),
                        fieldCount: 4,
                        castTo: x => [
                                         new BoundedValue<float>(x.Normal.X, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.Normal.Y, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.Normal.Z, float.MinValue, float.MaxValue),
                                         new BoundedValue<float>(x.D, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new System.Numerics.Plane(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2],
                                            d: (float)y[3]
                                         )
                    ),
                    fieldNames: [
                              new(nameof(System.Numerics.Vector3.X)),
                              new(nameof(System.Numerics.Vector3.Y)),
                              new(nameof(System.Numerics.Vector3.Z)),
                              new(nameof(System.Numerics.Plane.D))
                            ]
                ),
                isFallback: true);

            registry.RegisterType(typeof(System.Numerics.BigInteger),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.BigInteger>(
                        singleFieldType: typeof(System.Numerics.BigInteger),
                        x => [new UnboundedValue<System.Numerics.BigInteger>(x)],
                        y => (System.Numerics.BigInteger)y[0]
                    )),
                isFallback: true);

            registry.RegisterType(typeof(System.Numerics.Complex),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<System.Numerics.Complex>(
                        uniformFieldType: typeof(double),
                        fieldCount: 2,
                        castTo: x => [
                                         new BoundedValue<double>(x.Real, double.MinValue, double.MaxValue),
                                         new BoundedValue<double>(x.Imaginary, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new System.Numerics.Complex(
                                            real: (double)y[0],
                                            imaginary: (double)y[1]
                                       )
                    ),
                    fieldNames: [
                              new(nameof(System.Numerics.Complex.Real)),
                              new(nameof(System.Numerics.Complex.Imaginary))
                            ]
                ),
                isFallback: true);
        }
    }
}
