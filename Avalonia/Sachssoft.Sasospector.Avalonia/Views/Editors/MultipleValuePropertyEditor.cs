using Avalonia;
using Avalonia.Controls;
using Sachssoft.Sasospector.Adapters;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class MultipleValuePropertyEditor : InspectorPropertyEditorBase
    {
        public static readonly StyledProperty<MultipleValuePropertyAdapter<double>?> AdapterProperty =
            AvaloniaProperty.Register<ItemsControl, MultipleValuePropertyAdapter<double>?>(nameof(Adapter));

        public MultipleValuePropertyAdapter<double>? Adapter
        {
            get => GetValue(AdapterProperty);
            set => SetValue(AdapterProperty, value);
        }

    }
}
