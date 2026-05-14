using Sachssoft.Sasospector.Adapters;
using Sachssoft.Sasospector.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

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
            ColorPropertyAdapter adapter)
        {
            var editor = ((IColorEditor)f.CreateEditor(typeof(IColorEditor)));

            editor.IncludeAlpha = includeAlpha;
            editor.Adapter = adapter;

            return editor;
        }

        public static IPropertyEditor CreateMultipleValueEditor(
            this IInspectorEditorPlatformFactory f,
            int? defaultDecimalPlaces,
            IIndexedPropertyAdapter adapter,
            IReadOnlyList<string?>? fieldNames = null,
            Action<IMultipleNumericEditorField>? fieldSetups = null)
        {
            var editor = (IMultipleNumericEditor)f.CreateEditor(typeof(IMultipleNumericEditor));

            if (fieldNames != null && fieldNames.Count != adapter.FieldCount)
                throw new ArgumentException(
                    "The number of field names must match the adapter field count.",
                    nameof(fieldNames));

            fieldNames ??= Enumerable.Repeat<string?>(null, adapter.FieldCount)
                                     .ToList()
                                     .AsReadOnly();

            editor.Adapter = adapter;
            editor.Fields = Enumerable.Range(0, adapter.FieldCount)
                .Select(i =>
                {
                    var field = editor.CreateField(adapter.GetValueType(i));

                    field.Name = fieldNames[i];
                    field.DecimalPlaces = defaultDecimalPlaces;
                    fieldSetups?.Invoke(field);

                    return field;
                })
                .ToList()
                .AsReadOnly();

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

        public static IPropertyEditor CreateDateTimeEditor(
            this IInspectorEditorPlatformFactory f,
            DateTimeEditorParts parts,
            DateTimePropertyAdapter adapter)
        {
            var editor = ((IDateTimeEditor)f.CreateEditor(typeof(IDateTimeEditor)));

            editor.Adapter = adapter;
            editor.Parts = parts;

            return editor;
        }
    }
}
