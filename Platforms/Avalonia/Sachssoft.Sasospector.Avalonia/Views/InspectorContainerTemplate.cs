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

        public InspectorItem Build(object? param)
        {
            TemplateResult<Control>? templateResult = TemplateContent.Load(Content);

            if (templateResult?.Result is not InspectorItem)
                throw new InvalidOperationException();

            if (templateResult == null)
            {
                return null;
            }

            return (InspectorItem)templateResult.Result;
        }

        Control? ITemplate<object?, Control?>.Build(object? param)
        {
            return Build(param);
        }
    }
}
