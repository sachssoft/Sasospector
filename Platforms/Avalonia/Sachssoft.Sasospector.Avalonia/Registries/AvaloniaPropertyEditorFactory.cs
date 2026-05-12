using Sachssoft.Sasospector.Views.Editors;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Registries
{
    public class AvaloniaPropertyEditorFactory : IInspectorEditorPlatformFactory
    {
        private readonly Dictionary<Type, Func<InspectorPropertyEditorBase>> _factories = new();

        public AvaloniaPropertyEditorFactory()
        {
            RegisterDefaultEditors();
        }

        public void Register(Type type, Func<InspectorPropertyEditorBase> editor)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            _factories[type] = editor;
        }

        public InspectorPropertyEditorBase CreateEditor(Type editorType)
        {
            if (editorType == null)
                throw new ArgumentNullException(nameof(editorType));

            // Direkter Treffer
            if (_factories.TryGetValue(editorType, out var factory))
                return factory();

            // Interface-Suche
            if (editorType.IsInterface)
            {
                foreach (var entry in _factories)
                {
                    if (editorType.IsAssignableFrom(entry.Key))
                        return entry.Value();
                }
            }

            throw new InvalidOperationException(
                $"No editor registered for editor type '{editorType.FullName}'.");
        }

        IPropertyEditor IInspectorEditorPlatformFactory.CreateEditor(Type editorType)
        {
            return CreateEditor(editorType);
        }

        protected virtual void RegisterDefaultEditors()
        {
            Register(typeof(BigIntegerEditor), () => new BigIntegerEditor());
            Register(typeof(BooleanEditor), () => new BooleanEditor());
            Register(typeof(ColorEditor), () => new ColorEditor());
            Register(typeof(EnumEditor), () => new EnumEditor());
            Register(typeof(GuidEditor), () => new GuidEditor());
            Register(typeof(MultipleNumericEditor), () => new MultipleNumericEditor());
            //Register(typeof(NumberSliderPropertyEditor), () => new NumberSliderPropertyEditor());
            //Register(typeof(NumberSpinnerPropertyEditor), () => new NumberSpinnerPropertyEditor());
            Register(typeof(StringEditor), () => new StringEditor());
            Register(typeof(VersionEditor), () => new VersionEditor());
        }
    }
}