using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorContainerTemplate : ITemplate<object?, InspectorItem>, IDataTemplate
    {
        [DataType]
        public Type? DataType { get; set; }

        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        public bool Match(object? data)
        {
            if (DataType == null)
            {
                return true;
            }

            return DataType.IsInstanceOfType(data);
        }


        public InspectorItem? Build(object? param)
        {
            var templateResult = TemplateContent.Load(Content);

            if (templateResult?.Result is not InspectorItem item)
                return null;

            item.DataContext = param;

            return item;
        }

        Control? ITemplate<object?, Control?>.Build(object? param)
        {
            return Build(param);
        }
    }
}
