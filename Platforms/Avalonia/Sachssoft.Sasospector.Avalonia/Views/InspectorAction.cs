using Avalonia;
using System.Windows.Input;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorAction : AvaloniaObject
    {
        public static readonly StyledProperty<string?> TargetProperty =
            AvaloniaProperty.Register<InspectorAction, string?>(nameof(Target));

        public static readonly StyledProperty<object?> IconProperty =
            AvaloniaProperty.Register<InspectorAction, object?>(nameof(Icon));

        public static readonly StyledProperty<ICommand?> CommandProperty =
            AvaloniaProperty.Register<InspectorAction, ICommand?>(nameof(Command));

        public static readonly StyledProperty<object?> CommandParameterProperty =
            AvaloniaProperty.Register<InspectorAction, object?>(nameof(CommandParameter));

        public static readonly StyledProperty<InspectorItemBase?> RefreshItemProperty =
            AvaloniaProperty.Register<InspectorAction, InspectorItemBase?>(nameof(RefreshItem));

        public string? Target
        {
            get => GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        public object? Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public ICommand? Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public InspectorItemBase? RefreshItem
        {
            get => GetValue(RefreshItemProperty);
            set => SetValue(RefreshItemProperty, value);
        }
    }
}
