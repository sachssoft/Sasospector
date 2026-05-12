using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections.Generic;

namespace Sachssoft.Sasospector.Registries
{
    public static class EditorFactoryExtensions
    {
        public static IPropertyEditor CreateEnumEditor(
            this IInspectorEditorPlatformFactory f,
            EnumSelectionMode selectionMode)
        {
            var editor = ((IEnumEditor)f.CreateEditor(typeof(IEnumEditor)));

            editor.SelectionMode = selectionMode;

            return editor;
        }

        public static IPropertyEditor CreateColorEditor(
            this IInspectorEditorPlatformFactory f,
            bool includeAlpha,
            InspectorPropertyAdapterBase<ColorValue> adapter)
        {
            var editor = ((IColorEditor)f.CreateEditor(typeof(IColorEditor)));

            editor.IncludeAlpha = includeAlpha;
            editor.Adapter = adapter;

            return editor;
        }

        public static IPropertyEditor CreateMultipleValueEditor(
            this IInspectorEditorPlatformFactory f,
            int? decimalPlaces,
            InspectorPropertyAdapterBase<BoundedValue<double>[]> adapter,
            IReadOnlyList<EditorField>? fields = null)
        {
            var editor = ((IMultipleNumericEditor)f.CreateEditor(typeof(IMultipleNumericEditor)));

            editor.DecimalPlaces = decimalPlaces;
            editor.Adapter = adapter;
            editor.Fields = fields;

            return editor;
        }

        public static IPropertyEditor CreateStringEditor(
            this IInspectorEditorPlatformFactory f)
        {
            var editor = ((IStringEditor)f.CreateEditor(typeof(IStringEditor)));

            return editor;
        }

        public static IPropertyEditor CreateFileSystemEditor(
            this IInspectorEditorPlatformFactory f)
        {
            var editor = ((IFileSystemEditor)f.CreateEditor(typeof(IFileSystemEditor)));

            return editor;
        }

        public static IPropertyEditor CreateUriEditor(
            this IInspectorEditorPlatformFactory f)
        {
            var editor = ((IUriEditor)f.CreateEditor(typeof(IUriEditor)));

            return editor;
        }

        public static IPropertyEditor CreateVersionEditor(
            this IInspectorEditorPlatformFactory f)
        {
            var editor = ((IVersionEditor)f.CreateEditor(typeof(IVersionEditor)));

            return editor;
        }

        public static IPropertyEditor CreateGuidEditor(
            this IInspectorEditorPlatformFactory f)
        {
            var editor = ((IGuidEditor)f.CreateEditor(typeof(IGuidEditor)));

            return editor;
        }

        public static IPropertyEditor CreateBigIntegerEditor(
            this IInspectorEditorPlatformFactory f)
        {
            var editor = ((IBigIntegerEditor)f.CreateEditor(typeof(IBigIntegerEditor)));

            return editor;
        }

        public static IPropertyEditor CreateDateTimeEditor(
            this IInspectorEditorPlatformFactory f,
            DateTimeEditorParts parts,
            InspectorPropertyAdapterBase<DateTime> adapter)
        {
            var editor = ((IDateTimeEditor)f.CreateEditor(typeof(IDateTimeEditor)));

            editor.Adapter = adapter;
            editor.Parts = parts;

            return editor;
        }
    }
}
