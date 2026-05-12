using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace Sachssoft.Sasospector.Views.Templates
{
    public class InspectorSectionTemplate : ITemplate<object, InspectorSectionView>
    {
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        public InspectorSectionView Build(object categorySource)
        {
            var result = (InspectorSectionView)(TemplateContent.Load(Content)?.Result ?? new InspectorSectionView());
            result.DataContext = categorySource;
            return result;
        }
    }
}
