using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Sachssoft.Sasospector.Views.Editors
{
    public class ColorEditor : InspectorPropertyEditorBase, IColorEditor
    {

        public static readonly StyledProperty<InspectorPropertyAdapterBase<ColorValue>?> AdapterProperty =
            AvaloniaProperty.Register<ColorEditor, InspectorPropertyAdapterBase<ColorValue>?>(nameof(Adapter));

        public static readonly StyledProperty<bool> IncludeAlphaProperty =
            AvaloniaProperty.Register<ColorEditor, bool>(nameof(IncludeAlpha));

        public static readonly StyledProperty<Color> SelectedColorProperty =
            AvaloniaProperty.Register<ColorEditor, Color>(nameof(SelectedColor));

        protected override Type StyleKeyOverride { get; } = typeof(ColorEditor);

        public InspectorPropertyAdapterBase<ColorValue>? Adapter
        {
            get => GetValue(AdapterProperty);
            set => SetValue(AdapterProperty, value);
        }

        public bool IncludeAlpha
        {
            get => GetValue(IncludeAlphaProperty);
            set => SetValue(IncludeAlphaProperty, value);
        }

        public Color SelectedColor
        {
            get => GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
    }
}
