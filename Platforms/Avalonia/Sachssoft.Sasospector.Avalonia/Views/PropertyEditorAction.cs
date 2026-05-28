using Avalonia;
using System.Windows.Input;

namespace Sachssoft.Sasospector.Views
{
    public class PropertyEditorAction : AvaloniaObject
    {

        public static readonly StyledProperty<ICommand?> CommandProperty =
            AvaloniaProperty.Register<PropertyEditorAction, ICommand?>(nameof(Command));

        public static readonly StyledProperty<object?> CommandParameterProperty =
            AvaloniaProperty.Register<PropertyEditorAction, object?>(nameof(CommandParameter));

        public static readonly StyledProperty<string?> TargetProperty =
            AvaloniaProperty.Register<PropertyEditorAction, string?>(nameof(Target));

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

        public string? Target
        {
            get => GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }
    }
}
