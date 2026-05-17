using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using Sachssoft.Sasospector.Registries;
using System;

namespace Sachssoft.Sasospector.Views
{
    [TemplatePart(PART_PropertyGrid, typeof(ItemsControl))]
    public class InspectorControl : TemplatedControl
    {
        private const string PART_PropertyGrid = nameof(PART_PropertyGrid);

        private readonly AvaloniaList<InspectorItem> _items = new();
        private InspectorPropertyEditorRegistryBase _editorRegistry = AvaloniaPropertyEditorRegistry.Default;
        private ItemsControl? _partPropertyGrid = null;

        public static readonly DirectProperty<InspectorControl, InspectorPropertyEditorRegistryBase> EditorRegistryProperty =
            AvaloniaProperty.RegisterDirect<InspectorControl, InspectorPropertyEditorRegistryBase>(
                nameof(EditorRegistry),
                o => o.EditorRegistry,
                (o, v) => o.EditorRegistry = v);

        public static readonly StyledProperty<PropertyEditorModuleCollection?> EditorModulesProperty =
            AvaloniaProperty.Register<ItemsControl, PropertyEditorModuleCollection?>(nameof(EditorModules));

        //public static readonly StyledProperty<IEnumerable?> PropertiesSourceProperty =
        //    AvaloniaProperty.Register<InspectorControl, IEnumerable?>(nameof(PropertiesSource));

        //public static readonly StyledProperty<IEnumerable?> CategoriesSourceProperty =
        //    AvaloniaProperty.Register<InspectorControl, IEnumerable?>(nameof(CategoriesSource));

        //public static readonly StyledProperty<object?> MiscellaneousCategorySourceProperty =
        //    AvaloniaProperty.Register<InspectorControl, object?>(nameof(MiscellaneousCategorySource));

        //public static readonly StyledProperty<InspectorPropertyTemplate?> PropertyTemplateProperty =
        //    AvaloniaProperty.Register<InspectorControl, InspectorPropertyTemplate?>(nameof(PropertyTemplate));

        //public static readonly StyledProperty<InspectorPropertyGenerator?> PropertyGeneratorProperty =
        //    AvaloniaProperty.Register<InspectorControl, InspectorPropertyGenerator?>(nameof(PropertyGenerator));

        //public static readonly StyledProperty<InspectorSectionTemplate?> CategoryTemplateProperty =
        //    AvaloniaProperty.Register<InspectorControl, InspectorSectionTemplate?>(nameof(CategoryTemplate));

        //public static readonly StyledProperty<InspectorSectionGenerator?> CategoryGeneratorProperty =
        //    AvaloniaProperty.Register<InspectorControl, InspectorSectionGenerator?>(nameof(CategoryGenerator));

        //public static readonly StyledProperty<object?> SourceProperty =
        //    AvaloniaProperty.Register<ItemsControl, object?>(nameof(Source));

        public static readonly StyledProperty<double> PropertySpacingProperty =
            AvaloniaProperty.Register<ItemsControl, double>(nameof(PropertySpacing));

        public static readonly StyledProperty<double> SectionSpacingProperty =
            AvaloniaProperty.Register<ItemsControl, double>(nameof(SectionSpacing));

        public static readonly StyledProperty<double> HeaderWidthProperty =
            AvaloniaProperty.Register<ItemsControl, double>(nameof(HeaderWidth),
                defaultValue: 150.0);

        public InspectorControl()
        {
            SetValue(EditorRegistryProperty, AvaloniaPropertyEditorRegistry.Default);
        }

        protected override Type StyleKeyOverride { get; } = typeof(InspectorControl);

        [Content]
        public AvaloniaList<InspectorItem> Items => _items;

        public InspectorPropertyEditorRegistryBase EditorRegistry
        {
            get => _editorRegistry;
            set => SetAndRaise(EditorRegistryProperty, ref _editorRegistry,
                value ?? AvaloniaPropertyEditorRegistry.Default);
        }

        public PropertyEditorModuleCollection? EditorModules
        {
            get => GetValue(EditorModulesProperty);
            set => SetValue(EditorModulesProperty, value);
        }

        //public IEnumerable? PropertiesSource
        //{
        //    get => GetValue(PropertiesSourceProperty);
        //    set => SetValue(PropertiesSourceProperty, value);
        //}

        //public IEnumerable? CategoriesSource
        //{
        //    get => GetValue(CategoriesSourceProperty);
        //    set => SetValue(CategoriesSourceProperty, value);
        //}

        //public object? MiscellaneousCategorySource
        //{
        //    get => GetValue(MiscellaneousCategorySourceProperty);
        //    set => SetValue(MiscellaneousCategorySourceProperty, value);
        //}

        //public InspectorPropertyGenerator? PropertyGenerator
        //{
        //    get => GetValue(PropertyGeneratorProperty);
        //    set => SetValue(PropertyGeneratorProperty, value);
        //}

        //public InspectorPropertyTemplate? PropertyTemplate
        //{
        //    get => GetValue(PropertyTemplateProperty);
        //    set => SetValue(PropertyTemplateProperty, value);
        //}

        //public InspectorSectionGenerator? CategoryGenerator
        //{
        //    get => GetValue(CategoryGeneratorProperty);
        //    set => SetValue(CategoryGeneratorProperty, value);
        //}

        //public InspectorSectionTemplate? CategoryTemplate
        //{
        //    get => GetValue(CategoryTemplateProperty);
        //    set => SetValue(CategoryTemplateProperty, value);
        //}

        //public object? Source
        //{
        //    get => GetValue(SourceProperty);
        //    set => SetValue(SourceProperty, value);
        //}

        public double PropertySpacing
        {
            get => GetValue(PropertySpacingProperty);
            set => SetValue(PropertySpacingProperty, value);
        }

        public double SectionSpacing
        {
            get => GetValue(SectionSpacingProperty);
            set => SetValue(SectionSpacingProperty, value);
        }

        public double HeaderWidth
        {
            get => GetValue(HeaderWidthProperty);
            set => SetValue(HeaderWidthProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _partPropertyGrid = e.NameScope.Find<ItemsControl>(PART_PropertyGrid);

            //Build();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == EditorModulesProperty ||
                change.Property == EditorRegistryProperty)
            {
                if (EditorRegistry != null && EditorModules != null)
                {
                    foreach (var module in EditorModules)
                        EditorRegistry.TryAddModule(module);
                }
            }

            //if (change.Property == SourceProperty /*||*/
            ////change.Property == PropertiesSourceProperty ||
            ////change.Property == PropertyTemplateProperty ||
            ////change.Property == PropertyGeneratorProperty ||
            ////change.Property == CategoriesSourceProperty ||
            ////change.Property == MiscellaneousCategorySourceProperty ||
            ////change.Property == CategoryTemplateProperty ||
            ////change.Property == CategoryGeneratorProperty
            //)
            //{
            //    //Build();
            //}
        }

        //private void Build()
        //{
        //    if (_partPropertyGrid == null)
        //        return;

        //    if (PropertiesSource == null || Source == null)
        //    {
        //        _partPropertyGrid.ItemsSource = null;
        //        return;
        //    }

        //    var categories = BuildCategories();
        //    var properties = BuildProperties();

        //    // Kategorie-Container vorbereiten
        //    var grouped = categories.Values
        //        .ToDictionary(
        //            c => c.CategoryName,
        //            _ => new List<InspectorPropertyView>()
        //        );

        //    // Misc fallback
        //    grouped[string.Empty] = new List<InspectorPropertyView>();

        //    // Properties zuordnen
        //    foreach (var property in properties.Values)
        //    {
        //        var key = property.Source.Metadata.CategoryName;

        //        if (string.IsNullOrEmpty(key))
        //            key = string.Empty; // Misc

        //        if (!grouped.TryGetValue(key, out var list))
        //        {
        //            list = grouped[string.Empty];
        //        }

        //        list.Add(property);
        //    }

        //    // Views setzen (kein nested loop mehr!)
        //    foreach (var category in categories.Values)
        //    {
        //        grouped.TryGetValue(category.CategoryName, out var views);

        //        category.Views = views?.AsReadOnly();
        //    }

        //    _partPropertyGrid.ItemsSource =
        //        categories.Values.Where(c => c.Views?.Count() > 0);
        //}

        //private Dictionary<string, InspectorSectionView> BuildCategories()
        //{
        //    var categories = new Dictionary<string, InspectorSectionView>();

        //    if (CategoriesSource == null)
        //        return categories;

        //    foreach (var categorySource in CategoriesSource)
        //    {
        //        EnsureAddCategory(categorySource, categories);
        //    }

        //    if (MiscellaneousCategorySource != null)
        //        EnsureAddCategory(MiscellaneousCategorySource, categories);

        //    return categories;
        //}

        //private void EnsureAddCategory(object categorySource, Dictionary<string, InspectorSectionView> categories)
        //{
        //    var view = CreateCategoryView(categorySource);
        //    if (view == null)
        //        return;

        //    if (string.IsNullOrEmpty(view.CategoryName))
        //        throw new InvalidOperationException(
        //            $"Inspector category key is null or empty (Source: {categorySource?.GetType().Name ?? "unknown"}).");

        //    if (!categories.TryAdd(view.CategoryName, view))
        //        throw new InvalidOperationException(
        //            $"Duplicate inspector category key '{view.CategoryName}' (Source: {categorySource?.GetType().Name ?? "unknown"}).");
        //}

        //private Dictionary<string, InspectorPropertyView> BuildProperties()
        //{
        //    var properties = new Dictionary<string, InspectorPropertyView>();

        //    if (PropertiesSource == null)
        //        return properties;

        //    foreach (var propertySource in PropertiesSource)
        //    {
        //        var view = CreatePropertyView(propertySource);
        //        if (view == null)
        //            continue;

        //        var info = view.Source;

        //        //if (string.IsNullOrEmpty(view.PropertyName))
        //        //    throw new InvalidOperationException(
        //        //        $"Inspector property key is null or empty (Source: {propertySource?.GetType().Name ?? "unknown"}).");

        //        if (!properties.TryAdd(info.PropertyName, view))
        //            throw new InvalidOperationException(
        //                $"Duplicate inspector property key '{info.PropertyName}' (Source: {propertySource?.GetType().Name ?? "unknown"}).");
        //    }

        //    return properties;
        //}

        //private InspectorSectionView? CreateCategoryView(object categorySource)
        //{
        //    if (CategoryTemplate != null)
        //    {
        //        return CategoryTemplate.Build(categorySource);
        //    }

        //    if (categorySource is InspectorSectionView view)
        //    {
        //        return view;
        //    }
        //    else if (categorySource is InspectorPropertyCategoryInfo model)
        //    {
        //        return new InspectorSectionView
        //        {
        //            CategoryName = model.CategoryName,
        //            Header = model.CategoryName,
        //            DisplayOrder = model.DisplayOrder,
        //            IsToggleable = model.IsToggleable,
        //            DataContext = model
        //        };
        //    }
        //    else if (categorySource is string sourceName)
        //    {
        //        return new InspectorSectionView
        //        {
        //            CategoryName = sourceName,
        //            Header = sourceName,
        //            DataContext = sourceName
        //        };
        //    }

        //    var generator = CategoryGenerator ?? new InspectorSectionGenerator();
        //    return generator.Build(categorySource);
        //}

        //private InspectorPropertyView? CreatePropertyView(object propertySource)
        //{
        //    if (Source == null)
        //        return null;

        //    if (PropertyTemplate != null)
        //    {
        //        return PropertyTemplate.Build(Source, propertySource);
        //    }

        //    if (propertySource is InspectorPropertyView view)
        //    {
        //        return view;
        //    }
        //    else if (propertySource is InspectorPropertyInfo propertyInfo)
        //    {
        //        return new InspectorPropertyView
        //        {
        //            Source = propertyInfo,
        //            Header = propertyInfo.PropertyName
        //        };
        //    }
        //    else if (propertySource is PropertyInfo reflectionPropertyInfo)
        //    {
        //        return ReflectionPropertyEnumerator.GeneratePropertyView(Source, reflectionPropertyInfo);
        //    }

        //    var generator = PropertyGenerator ?? new InspectorPropertyGenerator();
        //    return generator.Build(Source, propertySource);
        //}

    }
}
