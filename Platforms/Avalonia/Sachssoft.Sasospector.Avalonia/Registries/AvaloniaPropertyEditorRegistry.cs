using System.Collections.Generic;

namespace Sachssoft.Sasospector.Registries
{
    public sealed class AvaloniaPropertyEditorRegistry : InspectorPropertyEditorRegistryBase
    {
        public AvaloniaPropertyEditorRegistry()
        {
        }

        public AvaloniaPropertyEditorRegistry(IEnumerable<IInspectorPropertyEditorModule> modules) : base(modules)
        {
        }

        public static AvaloniaPropertyEditorRegistry Default { get; }
            = new AvaloniaPropertyEditorRegistry(
                [
                    new CorePropertyEditorModule(),
                    new AvaloniaPropertyEditorModule()
                ]
            );

        protected override IInspectorEditorPlatformFactory CreatePlatformFactory()
        {
            return new AvaloniaPropertyEditorFactory();
        }
    }
}
