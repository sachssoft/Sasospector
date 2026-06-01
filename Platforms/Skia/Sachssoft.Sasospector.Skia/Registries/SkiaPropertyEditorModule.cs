using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Models;
using SkiaSharp;

namespace Sachssoft.Sasospector.Registries
{
    public sealed class SkiaPropertyEditorModule : IInspectorPropertyEditorModule
    {
        public void Register(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(SKColor),
                (f) => f.CreateColorEditor(
                    includeAlpha: true,
                    adapter: new ColorPropertyAdapter<SKColor>(
                        toField: (s) => new Color(s.Red, s.Green, s.Blue, s.Alpha),
                        toSource: (f) => new SKColor(f.Red, f.Green, f.Blue, f.Alpha)
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(SKColorF),
                (f) => f.CreateColorEditor(
                    includeAlpha: true,
                    adapter: new ColorPropertyAdapter<SKColorF>(
                        toField: (s) => new Color((byte)(s.Red * 255), (byte)(s.Green * 255), (byte)(s.Blue * 255), (byte)(s.Alpha * 255)),
                        toSource: (f) => new SKColorF(f.Red / 255f, f.Green / 255f, f.Blue / 255f, f.Alpha / 255f)
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(SKPMColor),
                f => f.CreateColorEditor(
                    includeAlpha: true,
                    adapter: new ColorPropertyAdapter<SKPMColor>(
                        toField: s => new Color(s.Red, s.Green, s.Blue, s.Alpha),
                        toSource: f => (SKPMColor)new SKColor(f.Red, f.Green, f.Blue, f.Alpha)
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(SKPoint),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<SKPoint>(
                        uniformFieldType: typeof(float),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<float>(x.X, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Y, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new SKPoint(
                                            x: (float)y[0],
                                            y: (float)y[1]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(SKPoint3),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<SKPoint3>(
                        uniformFieldType: typeof(float),
                        fieldCount: 3,
                        castTo: x => [
                                        new BoundedValue<float>(x.X, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Y, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Z, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new SKPoint3(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(SKPointI),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<SKPointI>(
                        uniformFieldType: typeof(int),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<int>(x.X, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Y, int.MinValue, int.MaxValue)
                                     ],
                        castFrom: y => new SKPointI(
                                            x: (int)y[0],
                                            y: (int)y[1]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(SKSize),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<SKSize>(
                        uniformFieldType: typeof(float),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<float>(x.Width, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Height, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new SKSize(
                                            width: (float)y[0],
                                            height: (float)y[1]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(SKSizeI),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<SKSizeI>(
                        uniformFieldType: typeof(int),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<int>(x.Width, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Height, int.MinValue, int.MaxValue)
                                     ],
                        castFrom: y => new SKSizeI(
                                            width: (int)y[0],
                                            height: (int)y[1]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(SKRect),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<SKRect>(
                        uniformFieldType: typeof(float),
                        fieldCount: 4,
                        castTo: x => [
                                        new BoundedValue<float>(x.Left, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Top, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Right, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Bottom, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new SKRect(
                                            left: (float)y[0],
                                            top: (float)y[1],
                                            right: (float)y[2],
                                            bottom: (float)y[3]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(SKRectI),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<SKRectI>(
                        uniformFieldType: typeof(int),
                        fieldCount: 4,
                        castTo: x => [
                                        new BoundedValue<int>(x.Left, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Top, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Right, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Bottom, int.MinValue, int.MaxValue)
                                     ],
                        castFrom: y => new SKRectI(
                                            left: (int)y[0],
                                            top: (int)y[1],
                                            right: (int)y[2],
                                            bottom: (int)y[3]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(SKMatrix),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<SKMatrix>(
                        uniformFieldType: typeof(float),
                        fieldCount: 9,
                        castTo: x => [
                                        new BoundedValue<float>(x.ScaleX, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.SkewX, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.TransX, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.SkewY, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.ScaleY, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.TransY, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Persp0, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Persp1, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Persp2, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new SKMatrix(
                                            scaleX: (float)y[0],
                                            skewX: (float)y[1],
                                            transX: (float)y[2],
                                            skewY: (float)y[3],
                                            scaleY: (float)y[4],
                                            transY: (float)y[5],
                                            persp0: (float)y[6],
                                            persp1: (float)y[7],
                                            persp2: (float)y[8]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(SKMatrix44),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<SKMatrix44>(
                        uniformFieldType: typeof(float),
                        fieldCount: 16,
                        castTo: x => [
                                        new BoundedValue<float>(x.M00, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M01, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M02, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M03, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M10, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M11, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M12, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M13, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M20, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M21, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M22, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M23, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M30, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M31, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M32, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.M33, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new SKMatrix44(
                                            m00: (float)y[0],
                                            m01: (float)y[1],
                                            m02: (float)y[2],
                                            m03: (float)y[3],
                                            m10: (float)y[4],
                                            m11: (float)y[5],
                                            m12: (float)y[6],
                                            m13: (float)y[7],
                                            m20: (float)y[8],
                                            m21: (float)y[9],
                                            m22: (float)y[10],
                                            m23: (float)y[11],
                                            m30: (float)y[12],
                                            m31: (float)y[13],
                                            m32: (float)y[14],
                                            m33: (float)y[15]
                                         )
                    )),
                isFallback: true);
        }
    }
}
