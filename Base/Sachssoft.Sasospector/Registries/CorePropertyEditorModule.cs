using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Editors;
using System;

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
            RegisterCommonTypes(registry);
            RegisterDataTimeTypes(registry);
            RegisterNumericMathTypes(registry);
        }

        private void RegisterRules(InspectorPropertyEditorRegistryBase registry)
        {
            //// ENUM
            //RegisterRule(
            //    type => type.IsEnum,
            //    info => new EnumPropertyEditor(),
            //    priority: 1000);

            //// NULLABLE
            //RegisterRule(
            //    type => Nullable.GetUnderlyingType(type) != null,
            //    info =>
            //    {
            //        var underlying = Nullable.GetUnderlyingType(info.Type)!;

            //        return new NullablePropertyEditor(
            //            underlying,
            //            Create(info with { Type = underlying }) // rekursiv
            //        );
            //    },
            //    priority: 900);

            //// ARRAY (optional)
            //RegisterRule(
            //    type => type.IsArray,
            //    info => new ArrayPropertyEditor(),
            //    priority: 800);
        }

        private void RegisterPrimitiveTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.Register(typeof(bool),
                (f) => f.CreateEditor(typeof(IBooleanSwitchEditor)),
                priority: 0);

            registry.Register(typeof(sbyte),
                (f) => f.CreateMultipleValueEditor(
                decimalPlaces: 0,
                adapter: new MultipleValuePropertyAdapter<double, sbyte>(
                    toValues: x => [new(x, sbyte.MinValue, sbyte.MaxValue)],
                    fromValues: y => (sbyte)y[0]
                )),
                priority: 0);

            registry.Register(typeof(sbyte),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, sbyte>(
                        x => [new(x, sbyte.MinValue, sbyte.MaxValue)],
                        y => (sbyte)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(short),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, short>(
                        x => [new(x, short.MinValue, short.MaxValue)],
                        y => (short)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(int),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, int>(
                        x => [new(x, int.MinValue, int.MaxValue)],
                        y => (int)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(long),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, long>(
                        x => [new(x, long.MinValue, long.MaxValue)],
                        y => (long)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(Int128),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, Int128>(
                        x => [new((double)x, (double)Int128.MinValue, (double)Int128.MaxValue)],
                        y => (Int128)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(byte),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, byte>(
                        x => [new(x, byte.MinValue, byte.MaxValue)],
                        y => (byte)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(ushort),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, ushort>(
                        x => [new(x, ushort.MinValue, ushort.MaxValue)],
                        y => (ushort)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(uint),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, uint>(
                        x => [new(x, uint.MinValue, uint.MaxValue)],
                        y => (uint)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(ulong),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, ulong>(
                        x => [new(x, ulong.MinValue, ulong.MaxValue)],
                        y => (ulong)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(Half),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: HALF_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, Half>(
                        x => [new((double)x, (double)Half.MinValue, (double)Half.MaxValue)],
                        y => (Half)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(float),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, float>(
                        x => [new(x, float.MinValue, float.MaxValue)],
                        y => (float)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(double),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, double>(
                        x => [new(x, double.MinValue, double.MaxValue)],
                        y => y[0]
                    )),
                priority: 0);

            registry.Register(typeof(decimal),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, decimal>(
                        x => [new((double)x, (double)decimal.MinValue, (double)decimal.MaxValue)],
                        y => (decimal)y[0]
                    )),
                priority: 0);
        }

        private void RegisterPlatformDependencyTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.Register(typeof(nint),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, nint>(
                        x => [new(x, nint.MinValue, nint.MaxValue)],
                        y => (nint)y[0]
                    )),
                priority: 0);

            registry.Register(typeof(nuint),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, nuint>(
                        x => [new(x, nuint.MinValue, nuint.MaxValue)],
                        y => (nuint)y[0]
                    )),
                priority: 0);
        }

        private void RegisterCommonTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.Register(typeof(string),
                (f) => f.CreateStringEditor(),
                priority: 0);

            registry.Register(typeof(char),
                (f) => f.CreateStringEditor(),
                priority: 0);

            registry.Register(typeof(Version),
                (f) => f.CreateVersionEditor(),
                priority: 0);

            registry.Register(typeof(Guid),
                (f) => f.CreateGuidEditor(),
                priority: 0);
        }

        private void RegisterDataTimeTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.Register(typeof(DateTime),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.DateTime,
                    adapter: null
                ),
                priority: 0);

            registry.Register(typeof(DateTimeOffset),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.DateTime,
                    adapter: null
                ),
                priority: 0);

            registry.Register(typeof(TimeSpan),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.Time,
                    adapter: null
                ),
                priority: 0);

            registry.Register(typeof(TimeOnly),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.Time,
                    adapter: null
                ),
                priority: 0);

            registry.Register(typeof(DateOnly),
                (f) => f.CreateDateTimeEditor(
                    parts: DateTimeEditorParts.Date,
                    adapter: null
                ),
                priority: 0);
        }

        private void RegisterNumericMathTypes(InspectorPropertyEditorRegistryBase registry)
        {
            registry.Register(typeof(System.Numerics.Vector2),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, System.Numerics.Vector2>(
                        toValues: x => [
                                         new(x.X, float.MinValue, float.MaxValue),
                                         new(x.Y, float.MinValue, float.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Vector2(
                                            x: (float)y[0],
                                            y: (float)y[1]
                                         )
                    ),
                    fields: [
                              new(nameof(System.Numerics.Vector2.X)),
                              new(nameof(System.Numerics.Vector2.Y))
                            ]
                ),
                priority: 0);

            registry.Register(typeof(System.Numerics.Vector3),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, System.Numerics.Vector3>(
                        toValues: x => [
                                         new(x.X, float.MinValue, float.MaxValue),
                                         new(x.Y, float.MinValue, float.MaxValue),
                                         new(x.Z, float.MinValue, float.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Vector3(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2]
                                         )
                    ),
                    fields: [
                              new(nameof(System.Numerics.Vector3.X)),
                              new(nameof(System.Numerics.Vector3.Y)),
                              new(nameof(System.Numerics.Vector3.Z))
                            ]
                ),
                priority: 0);

            registry.Register(typeof(System.Numerics.Vector4),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, System.Numerics.Vector4>(
                        toValues: x => [
                                         new(x.X, float.MinValue, float.MaxValue),
                                         new(x.Y, float.MinValue, float.MaxValue),
                                         new(x.Z, float.MinValue, float.MaxValue),
                                         new(x.W, float.MinValue, float.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Vector4(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2],
                                            w: (float)y[3]
                                         )
                    ),
                    fields: [
                              new(nameof(System.Numerics.Vector4.X)),
                              new(nameof(System.Numerics.Vector4.Y)),
                              new(nameof(System.Numerics.Vector4.Z)),
                              new(nameof(System.Numerics.Vector4.W))
                            ]
                ),
                priority: 0);

            registry.Register(typeof(System.Numerics.Matrix3x2),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, System.Numerics.Matrix3x2>(
                        toValues: x => [
                                         new(x.M11, float.MinValue, float.MaxValue),
                                         new(x.M12, float.MinValue, float.MaxValue),
                                         new(x.M21, float.MinValue, float.MaxValue),
                                         new(x.M22, float.MinValue, float.MaxValue),
                                         new(x.M31, float.MinValue, float.MaxValue),
                                         new(x.M32, float.MinValue, float.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Matrix3x2(
                                            m11: (float)y[0],
                                            m12: (float)y[1],
                                            m21: (float)y[2],
                                            m22: (float)y[3],
                                            m31: (float)y[4],
                                            m32: (float)y[5]
                                         )
                    ),
                    fields: [
                              new(nameof(System.Numerics.Matrix3x2.M11)),
                              new(nameof(System.Numerics.Matrix3x2.M12)),
                              new(nameof(System.Numerics.Matrix3x2.M21)),
                              new(nameof(System.Numerics.Matrix3x2.M22)),
                              new(nameof(System.Numerics.Matrix3x2.M31)),
                              new(nameof(System.Numerics.Matrix3x2.M32))
                            ]
                ),
                priority: 0);

            registry.Register(typeof(System.Numerics.Matrix4x4),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, System.Numerics.Matrix4x4>(
                        toValues: x => [
                                         new(x.M11, float.MinValue, float.MaxValue),
                                         new(x.M12, float.MinValue, float.MaxValue),
                                         new(x.M13, float.MinValue, float.MaxValue),
                                         new(x.M14, float.MinValue, float.MaxValue),
                                         new(x.M21, float.MinValue, float.MaxValue),
                                         new(x.M22, float.MinValue, float.MaxValue),
                                         new(x.M23, float.MinValue, float.MaxValue),
                                         new(x.M24, float.MinValue, float.MaxValue),
                                         new(x.M31, float.MinValue, float.MaxValue),
                                         new(x.M32, float.MinValue, float.MaxValue),
                                         new(x.M33, float.MinValue, float.MaxValue),
                                         new(x.M34, float.MinValue, float.MaxValue),
                                         new(x.M41, float.MinValue, float.MaxValue),
                                         new(x.M42, float.MinValue, float.MaxValue),
                                         new(x.M43, float.MinValue, float.MaxValue),
                                         new(x.M44, float.MinValue, float.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Matrix4x4(
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
                    fields: [
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
                priority: 0);

            registry.Register(typeof(System.Numerics.Quaternion),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, System.Numerics.Quaternion>(
                        toValues: x => [
                                         new(x.X, float.MinValue, float.MaxValue),
                                         new(x.Y, float.MinValue, float.MaxValue),
                                         new(x.Z, float.MinValue, float.MaxValue),
                                         new(x.W, float.MinValue, float.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Quaternion(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2],
                                            w: (float)y[3]
                                         )
                    ),
                    fields: [
                              new(nameof(System.Numerics.Quaternion.X)),
                              new(nameof(System.Numerics.Quaternion.Y)),
                              new(nameof(System.Numerics.Quaternion.Z)),
                              new(nameof(System.Numerics.Quaternion.W))
                            ]
                ),
                priority: 0);

            registry.Register(typeof(System.Numerics.Plane),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: FLOAT_DECIMAL_PLACES,
                    adapter: new MultipleValuePropertyAdapter<double, System.Numerics.Plane>(
                        toValues: x => [
                                         new(x.Normal.X, float.MinValue, float.MaxValue),
                                         new(x.Normal.Y, float.MinValue, float.MaxValue),
                                         new(x.Normal.Z, float.MinValue, float.MaxValue),
                                         new(x.D, float.MinValue, float.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Plane(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2],
                                            d: (float)y[3]
                                         )
                    ),
                    fields: [
                              new(nameof(System.Numerics.Vector3.X)),
                              new(nameof(System.Numerics.Vector3.Y)),
                              new(nameof(System.Numerics.Vector3.Z)),
                              new(nameof(System.Numerics.Plane.D))
                            ]
                ),
                priority: 0);

            registry.Register(typeof(System.Numerics.BigInteger),
                f => f.CreateBigIntegerEditor(),
                priority: 0);

            registry.Register(typeof(System.Numerics.Complex),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, System.Numerics.Complex>(
                        toValues: x => [
                                         new(x.Real, double.MinValue, double.MaxValue),
                                         new(x.Imaginary, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Complex(
                                            real: y[0],
                                            imaginary: y[1]
                                         )
                    ),
                    fields: [
                              new(nameof(System.Numerics.Complex.Real)),
                              new(nameof(System.Numerics.Complex.Imaginary))
                            ]
                ),
                priority: 0);
        }
    }
}
