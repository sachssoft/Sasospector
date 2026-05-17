using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace Sachssoft.Sasospector.Views.Templates
{
    public class InspectorSectionTemplate : ITemplate<object, SectionView>
    {
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        public SectionView Build(object categorySource)
        {
            var result = (SectionView)(TemplateContent.Load(Content)?.Result ?? new SectionView());
            result.DataContext = categorySource;
            return result;
        }
    }
}
