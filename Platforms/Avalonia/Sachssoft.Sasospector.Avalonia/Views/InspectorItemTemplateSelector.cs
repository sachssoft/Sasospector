using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Views
{
    public class InspectorItemTemplateSelector : IDataTemplate
    {
        [Content]
        public List<IDataTemplate> Templates { get; } = new();

        public InspectorItem Build(object? data)
        {
            foreach (var template in Templates)
            {
                if (template.Match(data))
                {
                    var control = template.Build(data);

                    if (control is InspectorItem item)
                        return item;

                    throw new InvalidOperationException(
                        $"Template returned '{control?.GetType().Name ?? "null"}' instead of '{nameof(InspectorItem)}'.");
                }
            }

            throw new InvalidOperationException(
                $"No matching template found for '{data?.GetType().Name ?? "null"}'.");
        }

        public bool Match(object? data)
        {
            return true;
        }

        Control? ITemplate<object?, Control?>.Build(object? param)
        {
            return Build(param);
        }
    }
}