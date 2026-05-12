using System;

namespace Sachssoft.Sasospector
{
    public interface IInspectorEditorPlatformFactory
    {
        IPropertyEditor CreateEditor(Type editorType);
    }
}
