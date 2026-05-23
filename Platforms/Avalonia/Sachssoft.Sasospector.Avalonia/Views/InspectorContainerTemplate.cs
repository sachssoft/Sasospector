using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using System;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorContainerTemplate : ITemplate<object?, InspectorItemBase>, IDataTemplate
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


        public InspectorItemBase? Build(object? param)
        {
            var templateResult = TemplateContent.Load(Content);

            if (templateResult?.Result is not InspectorItemBase item)
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
