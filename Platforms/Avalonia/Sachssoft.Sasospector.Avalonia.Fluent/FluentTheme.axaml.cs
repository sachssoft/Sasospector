using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaFluentTheme = Avalonia.Themes.Fluent.FluentTheme;

namespace Sachssoft.Sasospector.Views.Themes
{
    public class FluentTheme : AvaloniaFluentTheme, IResourceNode
    {
        public FluentTheme(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
        }
    }
}