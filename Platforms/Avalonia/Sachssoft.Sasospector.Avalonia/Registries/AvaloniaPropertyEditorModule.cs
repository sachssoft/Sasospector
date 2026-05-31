using Avalonia;
using Avalonia.Media;
using Sachssoft.Sasospector.Adapters;

namespace Sachssoft.Sasospector.Registries
{
    public sealed class AvaloniaPropertyEditorModule : IInspectorPropertyEditorModule
    {
        public void Register(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(Color),
                (f) => f.CreateColorEditor(
                    includeAlpha: true,
                    adapter: new ColorPropertyAdapter()
                ),
                priority: 0);

            registry.RegisterType(typeof(Avalonia.Vector),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Avalonia.Vector>(
                        uniformFieldType: typeof(double),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<double>(x.X, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Y, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new Avalonia.Vector(
                                            x: (double)y[0],
                                            y: (double)y[1]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(Vector3D),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Vector3D>(
                        uniformFieldType: typeof(double),
                        fieldCount: 3,
                        castTo: x => [
                                        new BoundedValue<double>(x.X, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Y, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Z, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new Vector3D(
                                            X: (double)y[0],
                                            Y: (double)y[1],
                                            Z: (double)y[2]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(Point),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Point>(
                        uniformFieldType: typeof(double),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<double>(x.X, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Y, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new Point(
                                            x: (double)y[0],
                                            y: (double)y[1]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(PixelPoint),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<PixelPoint>(
                        uniformFieldType: typeof(int),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<int>(x.X, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Y, int.MinValue, int.MaxValue)
                                     ],
                        castFrom: y => new PixelPoint(
                                            x: (int)y[0],
                                            y: (int)y[1]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(Size),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Size>(
                        uniformFieldType: typeof(double),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<double>(x.Width, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Height, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new Size(
                                            width: (double)y[0],
                                            height: (double)y[1]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(PixelSize),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<PixelSize>(
                        uniformFieldType: typeof(int),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<int>(x.Width, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Height, int.MinValue, int.MaxValue)
                                     ],
                        castFrom: y => new PixelSize(
                                            width: (int)y[0],
                                            height: (int)y[1]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(Rect),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Rect>(
                        uniformFieldType: typeof(double),
                        fieldCount: 4,
                        castTo: x => [
                                        new BoundedValue<double>(x.X, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Y, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Width, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Height, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new Rect(
                                            x: (double)y[0],
                                            y: (double)y[1],
                                            width: (double)y[2],
                                            height: (double)y[3]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(PixelRect),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: 0,
                    adapter: new IndexedFieldPropertyAdapter<PixelRect>(
                        uniformFieldType: typeof(int),
                        fieldCount: 4,
                        castTo: x => [
                                        new BoundedValue<int>(x.X, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Y, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Width, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Height, int.MinValue, int.MaxValue)
                                     ],
                        castFrom: y => new PixelRect(
                                            x: (int)y[0],
                                            y: (int)y[1],
                                            width: (int)y[2],
                                            height: (int)y[3]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(CornerRadius),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<CornerRadius>(
                        uniformFieldType: typeof(double),
                        fieldCount: 4,
                        castTo: x => [
                                        new BoundedValue<double>(x.TopLeft, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.TopRight, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.BottomRight, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.BottomLeft, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new CornerRadius(
                                            topLeft: (double)y[0],
                                            topRight: (double)y[1],
                                            bottomRight: (double)y[2],
                                            bottomLeft: (double)y[3]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(Thickness),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Thickness>(
                        uniformFieldType: typeof(double),
                        fieldCount: 4,
                        castTo: x => [
                                        new BoundedValue<double>(x.Left, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Top, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Right, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.Bottom, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new Thickness(
                                            left: (double)y[0],
                                            top: (double)y[1],
                                            right: (double)y[2],
                                            bottom: (double)y[3]
                                         )
                    )),
                priority: 0);

            registry.RegisterType(typeof(Matrix),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Matrix>(
                        uniformFieldType: typeof(double),
                        fieldCount: 9,
                        castTo: x => [
                                        new BoundedValue<double>(x.M11, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.M12, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.M13, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.M21, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.M22, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.M23, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.M31, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.M32, double.MinValue, double.MaxValue),
                                        new BoundedValue<double>(x.M33, double.MinValue, double.MaxValue)
                                     ],
                        castFrom: y => new Matrix(
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
                    )),
                priority: 0);

            //registry.Register(typeof(RelativePoint),
            //    () => new MultipleValuePropertyEditor
            //    {
            //        Adapter = new Adapters.MultipleValuePropertyAdapter<double>()
            //    },
            //    priority: 0);

            //registry.Register(typeof(RelativeRect),
            //    () => new MultipleValuePropertyEditor
            //    {
            //        Adapter = new Adapters.MultipleValuePropertyAdapter<double>()
            //    },
            //    priority: 0);

            //registry.Register(typeof(RoundedRect),
            //    () => new MultipleValuePropertyEditor
            //    {
            //        Adapter = new Adapters.MultipleValuePropertyAdapter<double>()
            //    },
            //    priority: 0);

            //registry.Register(typeof(RelativeScalar),
            //    () => new MultipleValuePropertyEditor
            //    {
            //        Adapter = new Adapters.MultipleValuePropertyAdapter<double>()
            //    },
            //    priority: 0);
        }
    }
}
