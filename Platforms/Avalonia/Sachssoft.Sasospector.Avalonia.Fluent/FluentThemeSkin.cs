using Avalonia;
using Avalonia.Media;

namespace Sachssoft.Sasospector.Views.Themes
{
    public class FluentThemeSkin : AvaloniaObject /*, IThemeSkin*/
    {
        private bool _isDefault = false;

        public static FluentThemeSkin Default { get; } = new FluentThemeSkin
        {
            _isDefault = true
        };

        public static readonly StyledProperty<Color> AccentProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(Accent));

        public static readonly StyledProperty<Color> AltHighProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(AltHigh));

        public static readonly StyledProperty<Color> AltLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(AltLow));

        public static readonly StyledProperty<Color> AltMediumProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(AltMedium));

        public static readonly StyledProperty<Color> AltMediumHighProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(AltMediumHigh));

        public static readonly StyledProperty<Color> AltMediumLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(AltMediumLow));

        public static readonly StyledProperty<Color> BaseHighProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(BaseHigh));

        public static readonly StyledProperty<Color> BaseMediumProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(BaseMedium));

        public static readonly StyledProperty<Color> BaseMediumHighProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(BaseMediumHigh));

        public static readonly StyledProperty<Color> BaseMediumLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(BaseMediumLow));

        public static readonly StyledProperty<Color> BaseLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(BaseLow));

        public static readonly StyledProperty<Color> ChromeAltLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeAltLow));

        public static readonly StyledProperty<Color> ChromeBlackHighProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeBlackHigh));

        public static readonly StyledProperty<Color> ChromeBlackMediumProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeBlackMedium));

        public static readonly StyledProperty<Color> ChromeBlackMediumLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeBlackMediumLow));

        public static readonly StyledProperty<Color> ChromeBlackLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeBlackLow));

        public static readonly StyledProperty<Color> ChromeDisabledHighProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeDisabledHigh));

        public static readonly StyledProperty<Color> ChromeDisabledLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeDisabledLow));

        public static readonly StyledProperty<Color> ChromeGrayProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeGray));

        public static readonly StyledProperty<Color> ChromeHighProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeHigh));

        public static readonly StyledProperty<Color> ChromeLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeLow));

        public static readonly StyledProperty<Color> ChromeMediumLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeMediumLow));

        public static readonly StyledProperty<Color> ChromeMediumProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeMedium));

        public static readonly StyledProperty<Color> ChromeWhiteProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ChromeWhite));

        public static readonly StyledProperty<Color> ErrorTextProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ErrorText));

        public static readonly StyledProperty<Color> ListLowProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ListLow));

        public static readonly StyledProperty<Color> ListMediumProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(ListMedium));

        public static readonly StyledProperty<Color> RegionColorProperty =
            AvaloniaProperty.Register<FluentThemeSkin, Color>(nameof(RegionColor));

        public Color Accent { get => GetValue(AccentProperty); set => SetValue(AccentProperty, value); }
        public Color AltHigh { get => GetValue(AltHighProperty); set => SetValue(AltHighProperty, value); }
        public Color AltLow { get => GetValue(AltLowProperty); set => SetValue(AltLowProperty, value); }
        public Color AltMedium { get => GetValue(AltMediumProperty); set => SetValue(AltMediumProperty, value); }
        public Color AltMediumHigh { get => GetValue(AltMediumHighProperty); set => SetValue(AltMediumHighProperty, value); }
        public Color AltMediumLow { get => GetValue(AltMediumLowProperty); set => SetValue(AltMediumLowProperty, value); }
        public Color BaseHigh { get => GetValue(BaseHighProperty); set => SetValue(BaseHighProperty, value); }
        public Color BaseMedium { get => GetValue(BaseMediumProperty); set => SetValue(BaseMediumProperty, value); }
        public Color BaseMediumHigh { get => GetValue(BaseMediumHighProperty); set => SetValue(BaseMediumHighProperty, value); }
        public Color BaseMediumLow { get => GetValue(BaseMediumLowProperty); set => SetValue(BaseMediumLowProperty, value); }
        public Color BaseLow { get => GetValue(BaseLowProperty); set => SetValue(BaseLowProperty, value); }
        public Color ChromeAltLow { get => GetValue(ChromeAltLowProperty); set => SetValue(ChromeAltLowProperty, value); }
        public Color ChromeBlackHigh { get => GetValue(ChromeBlackHighProperty); set => SetValue(ChromeBlackHighProperty, value); }
        public Color ChromeBlackMedium { get => GetValue(ChromeBlackMediumProperty); set => SetValue(ChromeBlackMediumProperty, value); }
        public Color ChromeBlackMediumLow { get => GetValue(ChromeBlackMediumLowProperty); set => SetValue(ChromeBlackMediumLowProperty, value); }
        public Color ChromeBlackLow { get => GetValue(ChromeBlackLowProperty); set => SetValue(ChromeBlackLowProperty, value); }
        public Color ChromeDisabledHigh { get => GetValue(ChromeDisabledHighProperty); set => SetValue(ChromeDisabledHighProperty, value); }
        public Color ChromeDisabledLow { get => GetValue(ChromeDisabledLowProperty); set => SetValue(ChromeDisabledLowProperty, value); }
        public Color ChromeGray { get => GetValue(ChromeGrayProperty); set => SetValue(ChromeGrayProperty, value); }
        public Color ChromeHigh { get => GetValue(ChromeHighProperty); set => SetValue(ChromeHighProperty, value); }
        public Color ChromeLow { get => GetValue(ChromeLowProperty); set => SetValue(ChromeLowProperty, value); }
        public Color ChromeMedium { get => GetValue(ChromeMediumProperty); set => SetValue(ChromeMediumProperty, value); }
        public Color ChromeMediumLow { get => GetValue(ChromeMediumLowProperty); set => SetValue(ChromeMediumLowProperty, value); }
        public Color ChromeWhite { get => GetValue(ChromeWhiteProperty); set => SetValue(ChromeWhiteProperty, value); }
        public Color ErrorText { get => GetValue(ErrorTextProperty); set => SetValue(ErrorTextProperty, value); }
        public Color ListLow { get => GetValue(ListLowProperty); set => SetValue(ListLowProperty, value); }
        public Color ListMedium { get => GetValue(ListMediumProperty); set => SetValue(ListMediumProperty, value); }
        public Color RegionColor { get => GetValue(RegionColorProperty); set => SetValue(RegionColorProperty, value); }

        //public void Apply(Styles? styles, ThemeSelectionContext context)
        //{
        //    var fluentTheme = styles?.OfType<AvaloniaFluentTheme>().FirstOrDefault();
        //    if (fluentTheme == null)
        //        return;

        //    fluentTheme.Palettes.Clear();

        //    if (!_isDefault && context.Variant != null)
        //    {
        //        fluentTheme.Palettes.Add(context.Variant, new ColorPaletteResources
        //        {
        //            Accent = Accent,
        //            AltHigh = AltHigh,
        //            AltLow = AltLow,
        //            AltMedium = AltMedium,
        //            AltMediumHigh = AltMediumHigh,
        //            AltMediumLow = AltMediumLow,
        //            BaseHigh = BaseHigh,
        //            BaseMedium = BaseMedium,
        //            BaseMediumHigh = BaseMediumHigh,
        //            BaseMediumLow = BaseMediumLow,
        //            BaseLow = BaseLow,
        //            ChromeAltLow = ChromeAltLow,
        //            ChromeBlackHigh = ChromeBlackHigh,
        //            ChromeBlackMedium = ChromeBlackMedium,
        //            ChromeBlackMediumLow = ChromeBlackMediumLow,
        //            ChromeBlackLow = ChromeBlackLow,
        //            ChromeDisabledHigh = ChromeDisabledHigh,
        //            ChromeDisabledLow = ChromeDisabledLow,
        //            ChromeGray = ChromeGray,
        //            ChromeHigh = ChromeHigh,
        //            ChromeLow = ChromeLow,
        //            ChromeMedium = ChromeMedium,
        //            ChromeMediumLow = ChromeMediumLow,
        //            ChromeWhite = ChromeWhite,
        //            ErrorText = ErrorText,
        //            ListLow = ListLow,
        //            ListMedium = ListMedium,
        //            RegionColor = RegionColor
        //        });
        //    }
        //}
    }
}
