using Microsoft.Xna.Framework;
using Sachssoft.Sasospector.Adapters;

namespace Sachssoft.Sasospector.Registries
{
    public sealed class MonogamePropertyEditorModule : IInspectorPropertyEditorModule
    {
        public void Register(InspectorPropertyEditorRegistryBase registry)
        {
            registry.RegisterType(typeof(Color),
                (f) => f.CreateColorEditor(
                    includeAlpha: true,
                    adapter: new ColorPropertyAdapter<Color>(
                        toField: s => new Sasospector.Models.Color(s.R, s.G, s.B, s.A),
                        toSource: f => new Color(f.Red, f.Green, f.Blue, f.Alpha)
                    )
                ),
                isFallback: true);

            registry.RegisterType(typeof(Vector2),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Vector2>(
                        uniformFieldType: typeof(float),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<float>(x.X, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Y, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new Vector2(
                                            x: (float)y[0],
                                            y: (float)y[1]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(Vector3),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Vector3>(
                        uniformFieldType: typeof(float),
                        fieldCount: 3,
                        castTo: x => [
                                        new BoundedValue<float>(x.X, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Y, float.MinValue, float.MaxValue),
                                        new BoundedValue<float>(x.Z, float.MinValue, float.MaxValue)
                                     ],
                        castFrom: y => new Vector3(
                                            x: (float)y[0],
                                            y: (float)y[1],
                                            z: (float)y[2]
                                         )
                    )),
                isFallback: true);

            registry.RegisterType(typeof(Point),
                f => f.CreateMultipleValueEditor(
                    defaultDecimalPlaces: null,
                    adapter: new IndexedFieldPropertyAdapter<Point>(
                        uniformFieldType: typeof(int),
                        fieldCount: 2,
                        castTo: x => [
                                        new BoundedValue<int>(x.X, int.MinValue, int.MaxValue),
                                        new BoundedValue<int>(x.Y, int.MinValue, int.MaxValue)
                                     ],
                        castFrom: y => new Point(
                                            x: (int)y[0],
                                            y: (int)y[1]
                                         )
                    )),
                isFallback: true);

        }
    }
}
