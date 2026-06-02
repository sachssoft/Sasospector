using Avalonia;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class CheckBoxPropertyViewItem : PropertyViewItem
    {

        public static readonly StyledProperty<bool> IsCheckedProperty =
            AvaloniaProperty.Register<CheckBoxPropertyViewItem, bool>(nameof(IsChecked));

        public static readonly StyledProperty<CheckBoxBehavior> CheckBehaviorProperty =
            AvaloniaProperty.Register<CheckBoxPropertyViewItem, CheckBoxBehavior>(nameof(CheckBehavior));

        protected override Type StyleKeyOverride => typeof(CheckBoxPropertyViewItem);

        public bool IsChecked
        {
            get => GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public CheckBoxBehavior CheckBehavior
        {
            get => GetValue(CheckBehaviorProperty);
            set => SetValue(CheckBehaviorProperty, value);
        }
    }
}
