using Avalonia.Controls.Templates;

namespace Sachssoft.Sasospector.Views.Editors
{
    public interface IItemTemplateProvider
    {

        IDataTemplate? ItemTemplate { get; set; }

    }
}
