using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using System;

namespace Sachssoft.Sasospector.Views.Templates
{
    public class InspectorPropertyTemplate : ITemplate<object, PropertyViewItem>
    {
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        public object? OwnerSource { get; private set; }

        public PropertyViewItem Build(object ownerSource, object propertySource)
        {
            OwnerSource = ownerSource;

            var result = (PropertyViewItem)(TemplateContent.Load(Content)?.Result ?? new PropertyViewItem());
            result.DataContext = propertySource;
            return result;
        }

        PropertyViewItem ITemplate<object, PropertyViewItem>.Build(object propertySource)
        {
            if (OwnerSource == null)
                throw new InvalidOperationException(
                    "InspectorPropertyTemplate: Owner must be set before calling Build().");

            return Build(OwnerSource, propertySource);
        }


    }
}
