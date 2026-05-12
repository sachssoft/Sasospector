using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using System;

namespace Sachssoft.Sasospector.Views.Editors
{
    [TemplatePart(PART_TextBox, typeof(TextBox))]
    public class GuidPropertyEditor : InspectorPropertyEditorBase
    {
        private const string PART_TextBox = nameof(PART_TextBox);

        private TextBox? _partTextBox = null;

        protected override Type StyleKeyOverride { get; } = typeof(GuidPropertyEditor);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partTextBox = e.NameScope.Get<TextBox>(PART_TextBox);
        }
    }
}
