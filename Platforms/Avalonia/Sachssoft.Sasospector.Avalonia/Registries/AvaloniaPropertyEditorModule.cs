using Avalonia;
using Avalonia.Media;
using Sachssoft.Sasospector.Adapters;

namespace Sachssoft.Sasospector.Registries
{
    public sealed class AvaloniaPropertyEditorModule : IInspectorPropertyEditorModule
    {
        public void Register(InspectorPropertyEditorRegistryBase registry)
        {
            registry.Register(typeof(Color),
                (f) => f.CreateColorEditor(
                    includeAlpha: true,
                    adapter: new ColorPropertyAdapter()
                ),
                priority: 0);

            registry.Register(typeof(Avalonia.Vector),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, Avalonia.Vector>(
                        toValues: x => [
                                         new(x.X, double.MinValue, double.MaxValue),
                                         new(x.Y, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Avalonia.Vector(
                                            x: (double)y[0],
                                            y: (double)y[1]
                                         )
                    )),
                priority: 0);

            registry.Register(typeof(Vector3D),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, Vector3D>(
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
                    )),
                priority: 0);

            registry.Register(typeof(Point),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, Point>(
                        toValues: x => [
                                         new(x.X, double.MinValue, double.MaxValue),
                                         new(x.Y, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Point(
                                            x: (double)y[0],
                                            y: (double)y[1]
                                         )
                    )),
                priority: 0);

            registry.Register(typeof(PixelPoint),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, PixelPoint>(
                        toValues: x => [
                                         new(x.X, int.MinValue, int.MaxValue),
                                         new(x.Y, int.MinValue, int.MaxValue)
                                       ],
                        fromValues: y => new PixelPoint(
                                            x: (int)y[0],
                                            y: (int)y[1]
                                         )
                    )),
                priority: 0);

            registry.Register(typeof(Size),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, Size>(
                        toValues: x => [
                                         new(x.Width, double.MinValue, double.MaxValue),
                                         new(x.Height, double.MinValue, double.MaxValue)
                                       ],
                        fromValues: y => new Size(
                                            width: (double)y[0],
                                            height: (double)y[1]
                                         )
                    )),
                priority: 0);

            registry.Register(typeof(PixelSize),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, PixelSize>(
                        toValues: x => [
                                         new(x.Width, int.MinValue, int.MaxValue),
                                         new(x.Height, int.MinValue, int.MaxValue)
                                       ],
                        fromValues: y => new PixelSize(
                                            width: (int)y[0],
                                            height: (int)y[1]
                                         )
                    )),
                priority: 0);

            registry.Register(typeof(Rect),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, Rect>(
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
                    )),
                priority: 0);

            registry.Register(typeof(PixelRect),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: 0,
                    adapter: new MultipleValuePropertyAdapter<double, PixelRect>(
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
                    )),
                priority: 0);

            registry.Register(typeof(CornerRadius),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, CornerRadius>(
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
                    )),
                priority: 0);

            registry.Register(typeof(Thickness),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, Thickness>(
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
                    )),
                priority: 0);

            registry.Register(typeof(Matrix),
                f => f.CreateMultipleValueEditor(
                    decimalPlaces: null,
                    adapter: new MultipleValuePropertyAdapter<double, Matrix>(
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
