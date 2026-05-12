using Avalonia;
using Avalonia.Media;
using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Views.Editors;
using System;
using System.Numerics;

namespace Sachssoft.Sasospector.Views
{
    public partial class InspectorPropertyEditorRegistry
    {
        private const int HALF_DECIMAL_PLACES = 3;
        private const int FLOAT_DECIMAL_PLACES = 6;

        private void RegisterRules()
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

        private void RegisterPrimitiveTypes()
        {
            Register(typeof(bool),
                () => new BooleanSwitchPropertyEditor(),
                priority: 0);

            Register(typeof(sbyte),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, sbyte>(
                        toValues: x => [new(x, sbyte.MinValue, sbyte.MaxValue)],
                        fromValues: y => (sbyte)y[0]
                    )
                },
                priority: 0);

            Register(typeof(short),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, short>(
                        toValues: x => [new(x, short.MinValue, short.MaxValue)],
                        fromValues: y => (short)y[0]
                    )
                },
                priority: 0);

            Register(typeof(int),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, int>(
                        toValues: x => [new(x, int.MinValue, int.MaxValue)],
                        fromValues: y => (int)y[0]
                    )
                },
                priority: 0);

            Register(typeof(long),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, long>(
                        toValues: x => [new(x, long.MinValue, long.MaxValue)],
                        fromValues: y => (long)y[0]
                    )
                },
                priority: 0);

            Register(typeof(Int128),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, Int128>(
                        toValues: x => [new((double)x, (double)Int128.MinValue, (double)Int128.MaxValue)],
                        fromValues: y => (Int128)y[0]
                    )
                },
                priority: 0);

            Register(typeof(byte),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, byte>(
                        toValues: x => [new(x, byte.MinValue, byte.MaxValue)],
                        fromValues: y => (byte)y[0]
                    )
                },
                priority: 0);

            Register(typeof(ushort),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, ushort>(
                        toValues: x => [new(x, ushort.MinValue, ushort.MaxValue)],
                        fromValues: y => (ushort)y[0]
                    )
                },
                priority: 0);

            Register(typeof(uint),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, uint>(
                        toValues: x => [new(x, uint.MinValue, uint.MaxValue)],
                        fromValues: y => (uint)y[0]
                    )
                },
                priority: 0);

            Register(typeof(ulong),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, ulong>(
                        toValues: x => [new(x, ulong.MinValue, ulong.MaxValue)],
                        fromValues: y => (ulong)y[0]
                    )
                },
                priority: 0);

            Register(typeof(Half),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = HALF_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, Half>(
                        toValues: x => [new((double)x, (double)Half.MinValue, (double)Half.MaxValue)],
                        fromValues: y => (Half)y[0]
                    )
                },
                priority: 0);

            Register(typeof(float),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = FLOAT_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, float>(
                        toValues: x => [new(x, float.MinValue, float.MaxValue)],
                        fromValues: y => (float)y[0]
                    )
                },
                priority: 0);

            Register(typeof(double),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, double>(
                        toValues: x => [new(x, double.MinValue, double.MaxValue)],
                        fromValues: y => y[0]
                    )
                },
                priority: 0);

            Register(typeof(decimal),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, decimal>(
                        toValues: x => [new((double)x, (double)decimal.MinValue, (double)decimal.MaxValue)],
                        fromValues: y => (decimal)y[0]
                    )
                },
                priority: 0);
        }

        private void RegisterPlatformDependencyTypes()
        {
            Register(typeof(nint),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, nint>(
                        toValues: x => [new(x, nint.MinValue, nint.MaxValue)],
                        fromValues: y => (nint)y[0]
                    )
                },
                priority: 0);

            Register(typeof(nuint),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, nuint>(
                        toValues: x => [new(x, nuint.MinValue, nuint.MaxValue)],
                        fromValues: y => (nuint)y[0]
                    )
                },
                priority: 0);
        }

        private void RegisterCommonTypes()
        {
            Register(typeof(string),
                () => new TextPropertyEditor(),
                priority: 0);

            Register(typeof(char),
                () => new TextPropertyEditor(),
                priority: 0);

            Register(typeof(Version),
                () => new VersionPropertyEditor(),
                priority: 0);

            Register(typeof(Guid),
                () => new TextPropertyEditor(),
                priority: 0);
        }

        private void RegisterDataTimeTypes()
        {
            Register(typeof(DateTime),
                () => new TextPropertyEditor(),
                priority: 0);

            Register(typeof(DateTimeOffset),
                () => new TextPropertyEditor(),
                priority: 0);

            Register(typeof(TimeSpan),
                () => new VersionPropertyEditor(),
                priority: 0);

            Register(typeof(TimeOnly),
                () => new TextPropertyEditor(),
                priority: 0);

            Register(typeof(DateOnly),
                () => new TextPropertyEditor(),
                priority: 0);
        }

        private void RegisterNumericMathTypes()
        {
            Register(typeof(System.Numerics.Vector2),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = FLOAT_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, System.Numerics.Vector2>(
                        toValues: x => [
                                         new(x.X, float.MinValue, float.MaxValue),
                                         new(x.Y, float.MinValue, float.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Vector2(
                                            x: (float)y[0],
                                            y: (float)y[1]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(System.Numerics.Vector3),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = FLOAT_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, System.Numerics.Vector3>(
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
                    )
                },
                priority: 0);

            Register(typeof(System.Numerics.Vector4),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = FLOAT_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, System.Numerics.Vector4>(
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
                    )
                },
                priority: 0);

            Register(typeof(System.Numerics.Matrix3x2),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = FLOAT_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, System.Numerics.Matrix3x2>(
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
                    )
                },
                priority: 0);

            Register(typeof(System.Numerics.Matrix4x4),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = FLOAT_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, System.Numerics.Matrix4x4>(
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
                    )
                },
                priority: 0);

            Register(typeof(System.Numerics.Quaternion),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = FLOAT_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, System.Numerics.Quaternion>(
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
                    )
                },
                priority: 0);

            Register(typeof(System.Numerics.Plane),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = FLOAT_DECIMAL_PLACES,
                    Adapter = new MultipleValuePropertyAdapter<double, System.Numerics.Plane>(
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
                    )
                },
                priority: 0);

            Register(typeof(System.Numerics.BigInteger),
                () => new BigIntegerEditor(),
                priority: 0);

            Register(typeof(System.Numerics.Complex),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, System.Numerics.Complex>(
                        toValues: x => [
                                         new(x.Real, double.MinValue, double.MaxValue),
                                         new(x.Imaginary, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new System.Numerics.Complex(
                                            real: y[0],
                                            imaginary: y[1]
                                         )
                    )
                },
                priority: 0);
        }

        private void RegisterAvaloniaTypes()
        {
            Register(typeof(Color),
                () => new ColorPropertyEditor(),
                priority: 0);

            Register(typeof(Avalonia.Vector),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, Avalonia.Vector>(
                        toValues: x => [
                                         new(x.X, double.MinValue, double.MaxValue),
                                         new(x.Y, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Avalonia.Vector(
                                            x: (double)y[0],
                                            y: (double)y[1]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(Vector3D),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, Vector3D>(
                        toValues: x => [
                                         new(x.X, double.MinValue, double.MaxValue),
                                         new(x.Y, double.MinValue, double.MaxValue),
                                         new(x.Z, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Vector3D(
                                            X: (double)y[0],
                                            Y: (double)y[1],
                                            Z: (double)y[2]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(Point),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, Point>(
                        toValues: x => [
                                         new(x.X, double.MinValue, double.MaxValue),
                                         new(x.Y, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Point(
                                            x: (double)y[0],
                                            y: (double)y[1]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(PixelPoint),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, PixelPoint>(
                        toValues: x => [
                                         new(x.X, int.MinValue, int.MaxValue),
                                         new(x.Y, int.MinValue, int.MaxValue)
                                       ],
                        fromValues: y => new PixelPoint(
                                            x: (int)y[0],
                                            y: (int)y[1]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(Size),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, Size>(
                        toValues: x => [
                                         new(x.Width, double.MinValue, double.MaxValue),
                                         new(x.Height, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Size(
                                            width: (double)y[0],
                                            height: (double)y[1]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(PixelSize),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, PixelSize>(
                        toValues: x => [
                                         new(x.Width, int.MinValue, int.MaxValue),
                                         new(x.Height, int.MinValue, int.MaxValue)
                                       ],
                        fromValues: y => new PixelSize(
                                            width: (int)y[0],
                                            height: (int)y[1]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(Rect),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, Rect>(
                        toValues: x => [
                                         new(x.X, double.MinValue, double.MaxValue),
                                         new(x.Y, double.MinValue, double.MaxValue),
                                         new(x.Width, double.MinValue, double.MaxValue),
                                         new(x.Height, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Rect(
                                            x: (double)y[0],
                                            y: (double)y[1],
                                            width: (double)y[2],
                                            height: (double)y[3]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(PixelRect),
                () => new MultipleValuePropertyEditor
                {
                    DecimalPlaces = 0,
                    Adapter = new MultipleValuePropertyAdapter<double, PixelRect>(
                        toValues: x => [
                                         new(x.X, int.MinValue, int.MaxValue),
                                         new(x.Y, int.MinValue, int.MaxValue),
                                         new(x.Width, int.MinValue, int.MaxValue),
                                         new(x.Height, int.MinValue, int.MaxValue)
                                       ],
                        fromValues: y => new PixelRect(
                                            x: (int)y[0],
                                            y: (int)y[1],
                                            width: (int)y[2],
                                            height: (int)y[3]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(CornerRadius),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, CornerRadius>(
                        toValues: x => [
                                         new(x.TopLeft, double.MinValue, double.MaxValue),
                                         new(x.TopRight, double.MinValue, double.MaxValue),
                                         new(x.BottomRight, double.MinValue, double.MaxValue),
                                         new(x.BottomLeft, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new CornerRadius(
                                            topLeft: (double)y[0],
                                            topRight: (double)y[1],
                                            bottomRight: (double)y[2],
                                            bottomLeft: (double)y[3]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(Thickness),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, Thickness>(
                        toValues: x => [
                                         new(x.Left, double.MinValue, double.MaxValue),
                                         new(x.Top, double.MinValue, double.MaxValue),
                                         new(x.Right, double.MinValue, double.MaxValue),
                                         new(x.Bottom, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Thickness(
                                            left: (double)y[0],
                                            top: (double)y[1],
                                            right: (double)y[2],
                                            bottom: (double)y[3]
                                         )
                    )
                },
                priority: 0);

            Register(typeof(Matrix),
                () => new MultipleValuePropertyEditor
                {
                    Adapter = new MultipleValuePropertyAdapter<double, Matrix>(
                        toValues: x => [
                                         new(x.M11, double.MinValue, double.MaxValue),
                                         new(x.M12, double.MinValue, double.MaxValue),
                                         new(x.M13, double.MinValue, double.MaxValue),
                                         new(x.M21, double.MinValue, double.MaxValue),
                                         new(x.M22, double.MinValue, double.MaxValue),
                                         new(x.M23, double.MinValue, double.MaxValue),
                                         new(x.M31, double.MinValue, double.MaxValue),
                                         new(x.M32, double.MinValue, double.MaxValue),
                                         new(x.M33, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Matrix(
                                            scaleX: (double)y[0],
                                            skewY: (double)y[1],
                                            perspX: (double)y[2],
                                            skewX: (double)y[3],
                                            scaleY: (double)y[4],
                                            perspY: (double)y[5],
                                            offsetX: (double)y[6],
                                            offsetY: (double)y[7],
                                            perspZ: (double)y[8]
                                         )
                    )
                },
                priority: 0);

            //Register(typeof(RelativePoint),
            //    () => new MultipleValuePropertyEditor
            //    {
            //        Adapter = new Adapters.MultipleValuePropertyAdapter<double>()
            //    },
            //    priority: 0);

            //Register(typeof(RelativeRect),
            //    () => new MultipleValuePropertyEditor
            //    {
            //        Adapter = new Adapters.MultipleValuePropertyAdapter<double>()
            //    },
            //    priority: 0);

            //Register(typeof(RoundedRect),
            //    () => new MultipleValuePropertyEditor
            //    {
            //        Adapter = new Adapters.MultipleValuePropertyAdapter<double>()
            //    },
            //    priority: 0);

            //Register(typeof(RelativeScalar),
            //    () => new MultipleValuePropertyEditor
            //    {
            //        Adapter = new Adapters.MultipleValuePropertyAdapter<double>()
            //    },
            //    priority: 0);
        }
    }
}