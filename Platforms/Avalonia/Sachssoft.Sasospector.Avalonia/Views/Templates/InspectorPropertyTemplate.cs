using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using System;

namespace Sachssoft.Sasospector.Views.Templates
{
    public class InspectorPropertyTemplate : ITemplate<object, InspectorPropertyView>
    {
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        public object? OwnerSource { get; private set; }

        public InspectorPropertyView Build(object ownerSource, object propertySource)
        {
            OwnerSource = ownerSource;

            var result = (InspectorPropertyView)(TemplateContent.Load(Content)?.Result ?? new InspectorPropertyView());
            result.DataContext = propertySource;
            return result;
        }

        InspectorPropertyView ITemplate<object, InspectorPropertyView>.Build(object propertySource)
        {
            if (OwnerSource == null)
                throw new InvalidOperationException(
                    "InspectorPropertyTemplate: Owner must be set before calling Build().");

            return Build(OwnerSource, propertySource);
        }


    }
}
