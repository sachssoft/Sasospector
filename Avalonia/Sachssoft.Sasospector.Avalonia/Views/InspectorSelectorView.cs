using Avalonia.Controls.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorSelectorView : InspectorItem
    {
        public InspectorSelectorView() { }

        public IDataTemplate DataTemplate
        {
            get;set;
        }
    }
}
