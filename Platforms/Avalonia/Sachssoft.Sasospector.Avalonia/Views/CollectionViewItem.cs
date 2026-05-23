using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Views
{
    [TemplatePart(PART_Items, typeof(ItemsControl))]
    public class CollectionViewItem : InspectorItemBase
    {
        private const string PART_Items = nameof(PART_Items);

        private ItemsControl? _partItems;

        protected override Type StyleKeyOverride => typeof(CollectionViewItem);

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partItems = e.NameScope.Get<ItemsControl>(PART_Items);
            InvalidateProperty();
        }

        protected override void OnPropertyInvalidated(IInspectorPropertyInfo propertyInfo)
        {
            base.OnPropertyInvalidated(propertyInfo);

            if (_partItems == null)
                return;

            var enumerable = (propertyInfo.GetValue() as IEnumerable)
                             ?? Array.Empty<object?>();

            _partItems.ItemsSource = enumerable;

            //var list = new List<InspectorItem>();

            //foreach (var item in enumerable)
            //{
            //    var template = ResolveTemplate(item);

            //    if (template == null)
            //        continue;

            //    var instance = template.Build(item);

            //    if (instance is not InspectorItem inspectorItem)
            //        throw new InvalidOperationException("Template must produce InspectorItem");

            //    inspectorItem.DataContext = item;

            //    var testTag = inspectorItem.Tag;

            //    list.Add(inspectorItem);
            //}

            //_partItems.ItemsSource = list;
        }

        //private IDataTemplate? ResolveTemplate(object? value)
        //{
        //    // 1. explizites ItemTemplate (höchste Priorität)
        //    if (ItemTemplate != null)
        //        return ItemTemplate;

        //    // 2. lokale DataTemplates (richtig gefiltert!)
        //    var localTemplate = DataTemplates?
        //        .OfType<IDataTemplate>()
        //        .FirstOrDefault(t => MatchesTemplate(t, value));

        //    if (localTemplate != null)
        //        return localTemplate;

        //    // 3. Resources fallback (FEHLT bei dir)
        //    if (Resources != null)
        //    {
        //        var resourceTemplate = Resources.Values
        //            .OfType<IDataTemplate>()
        //            .FirstOrDefault(t => MatchesTemplate(t, value));

        //        if (resourceTemplate != null)
        //            return resourceTemplate;
        //    }

        //    return null;
        //}

        //private bool MatchesTemplate(IDataTemplate template, object? value)
        //{
        //    if (template == null)
        //        return false;

        //    if (value == null)
        //        return true; // optional fallback

        //    // 1. Wenn Template Type-basiert ist (typisch in Avalonia)
        //    if (template is IDataTemplate typed)
        //    {
        //        return typed.Match(value);
        //    }

        //    // 2. fallback: keine harte Entscheidung erzwingen
        //    return true;
        //}
    }
}
