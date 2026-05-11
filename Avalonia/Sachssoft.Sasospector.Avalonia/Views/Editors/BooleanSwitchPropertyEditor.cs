using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using System;

namespace Sachssoft.Sasospector.Views.Editors
{
    [TemplatePart(PART_Switch, typeof(ToggleSwitch))]
    public class BooleanSwitchPropertyEditor : InspectorPropertyEditorBase
    {
        private const string PART_Switch = nameof(PART_Switch);

        private ToggleSwitch? _partSwitch = null;

        protected override Type StyleKeyOverride { get; } = typeof(BooleanSwitchPropertyEditor);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partSwitch = e.NameScope.Get<ToggleSwitch>(PART_Switch);
        }
    }
}
