using System.Windows;
using System.Windows.Media;
using WigglerBySmba.Models;
using AppThemeMode = WigglerBySmba.Models.ThemeMode;
using AppThemeVibe = WigglerBySmba.Models.ThemeVibe;
using MediaColor = System.Windows.Media.Color;

namespace WigglerBySmba.Services;

public sealed class ThemeService
{
    public void ApplyTheme(AppThemeMode mode, AppThemeVibe vibe)
    {
        var palette = BuildPalette(mode, vibe);
        var resources = System.Windows.Application.Current.Resources;

        SetBrush(resources, "WindowBackgroundBrush", palette.WindowBackground);
        SetBrush(resources, "PanelBrush", palette.Panel);
        SetBrush(resources, "SurfaceBrush", palette.Surface);
        SetBrush(resources, "SurfaceMutedBrush", palette.SurfaceMuted);
        SetBrush(resources, "AccentBrush", palette.Accent);
        SetBrush(resources, "AccentMutedBrush", palette.AccentMuted);
        SetBrush(resources, "TextBrush", palette.Text);
        SetBrush(resources, "SubtleTextBrush", palette.SubtleText);
        SetBrush(resources, "FrameBrush", palette.Frame);
        SetBrush(resources, "ScreenBrush", palette.Screen);
        SetBrush(resources, "SettingsBackgroundBrush", palette.SettingsBackground);
        SetBrush(resources, "SettingsPanelBrush", palette.SettingsPanel);
        SetBrush(resources, "ThemeChipBrush", palette.ThemeChip);
        SetBrush(resources, "ThemeChipBorderBrush", palette.ThemeChipBorder);
        SetBrush(resources, "HeroWashBrush", palette.HeroWash);
        SetBrush(resources, "HeroCardBrush", palette.HeroCard);
        SetBrush(resources, "HeroOutlineBrush", palette.HeroOutline);
        SetBrush(resources, "ButtonTextBrush", palette.ButtonText);
    }

    private static void SetBrush(ResourceDictionary resources, string key, MediaColor color)
    {
        resources[key] = new SolidColorBrush(color);
    }

    private static ThemePalette BuildPalette(AppThemeMode mode, AppThemeVibe vibe)
    {
        return (mode, vibe) switch
        {
            (AppThemeMode.Dark, AppThemeVibe.Tide) => new(
                ColorFromHex("#090A0F"),
                ColorFromHex("#0F1118"),
                ColorFromHex("#161A23"),
                ColorFromHex("#12161F"),
                ColorFromHex("#4FD1B4"),
                ColorFromHex("#16342E"),
                ColorFromHex("#F5F7FB"),
                ColorFromHex("#98A0B3"),
                ColorFromHex("#242936"),
                ColorFromHex("#0F121A"),
                ColorFromHex("#10141C"),
                ColorFromHex("#111520"),
                ColorFromHex("#181D28"),
                ColorFromHex("#272D39"),
                ColorFromHex("#151924"),
                ColorFromHex("#161B26"),
                ColorFromHex("#F5F7FB"),
                ColorFromHex("#F5F7FB")),
            (AppThemeMode.Dark, AppThemeVibe.Ember) => new(
                ColorFromHex("#0A0B10"),
                ColorFromHex("#11131A"),
                ColorFromHex("#191C24"),
                ColorFromHex("#131720"),
                ColorFromHex("#D89A72"),
                ColorFromHex("#382720"),
                ColorFromHex("#FBF6F3"),
                ColorFromHex("#A59DA5"),
                ColorFromHex("#252A36"),
                ColorFromHex("#10131B"),
                ColorFromHex("#11151C"),
                ColorFromHex("#121720"),
                ColorFromHex("#1A1E27"),
                ColorFromHex("#2B313D"),
                ColorFromHex("#171A24"),
                ColorFromHex("#181D27"),
                ColorFromHex("#FBF6F3"),
                ColorFromHex("#FBF6F3")),
            (AppThemeMode.Dark, AppThemeVibe.Citrus) => new(
                ColorFromHex("#090B0E"),
                ColorFromHex("#10141A"),
                ColorFromHex("#181D22"),
                ColorFromHex("#13191D"),
                ColorFromHex("#B4C96D"),
                ColorFromHex("#313721"),
                ColorFromHex("#F7FAF1"),
                ColorFromHex("#9AA294"),
                ColorFromHex("#242A31"),
                ColorFromHex("#10151A"),
                ColorFromHex("#11161A"),
                ColorFromHex("#12181C"),
                ColorFromHex("#1B2126"),
                ColorFromHex("#2C3439"),
                ColorFromHex("#181D22"),
                ColorFromHex("#191F23"),
                ColorFromHex("#F7FAF1"),
                ColorFromHex("#F7FAF1")),
            (AppThemeMode.Dark, AppThemeVibe.Bloom) => new(
                ColorFromHex("#09090F"),
                ColorFromHex("#11111A"),
                ColorFromHex("#191925"),
                ColorFromHex("#13141E"),
                ColorFromHex("#B88CF8"),
                ColorFromHex("#2C2141"),
                ColorFromHex("#F7F3FB"),
                ColorFromHex("#A69CB5"),
                ColorFromHex("#252638"),
                ColorFromHex("#10111A"),
                ColorFromHex("#11121C"),
                ColorFromHex("#121421"),
                ColorFromHex("#1A1C2A"),
                ColorFromHex("#2C2F3E"),
                ColorFromHex("#171826"),
                ColorFromHex("#181A28"),
                ColorFromHex("#F7F3FB"),
                ColorFromHex("#F7F3FB")),
            (AppThemeMode.Light, AppThemeVibe.Ember) => new(
                ColorFromHex("#EEEBF3"),
                ColorFromHex("#F9F7FC"),
                ColorFromHex("#FFFFFF"),
                ColorFromHex("#F2EEF6"),
                ColorFromHex("#D17A47"),
                ColorFromHex("#F5DED3"),
                ColorFromHex("#202534"),
                ColorFromHex("#71788B"),
                ColorFromHex("#D4CEDF"),
                ColorFromHex("#F6F4FB"),
                ColorFromHex("#FAF8FD"),
                ColorFromHex("#FFFEFF"),
                ColorFromHex("#EEEAF4"),
                ColorFromHex("#D7D0E2"),
                ColorFromHex("#FAF8FD"),
                ColorFromHex("#FFFEFF"),
                ColorFromHex("#202534"),
                ColorFromHex("#FFFFFF")),
            (AppThemeMode.Light, AppThemeVibe.Citrus) => new(
                ColorFromHex("#EFEDF2"),
                ColorFromHex("#F9F8FC"),
                ColorFromHex("#FFFFFF"),
                ColorFromHex("#F2F0F5"),
                ColorFromHex("#95A93D"),
                ColorFromHex("#E3EABF"),
                ColorFromHex("#202534"),
                ColorFromHex("#727A86"),
                ColorFromHex("#D4D1DD"),
                ColorFromHex("#F6F4FA"),
                ColorFromHex("#FAF8FD"),
                ColorFromHex("#FFFEFF"),
                ColorFromHex("#EEEBF3"),
                ColorFromHex("#D7D3E0"),
                ColorFromHex("#FAF8FD"),
                ColorFromHex("#FFFEFF"),
                ColorFromHex("#202534"),
                ColorFromHex("#FFFFFF")),
            (AppThemeMode.Light, AppThemeVibe.Bloom) => new(
                ColorFromHex("#EEEAF5"),
                ColorFromHex("#F9F7FD"),
                ColorFromHex("#FFFFFF"),
                ColorFromHex("#F2EEF8"),
                ColorFromHex("#8F69F2"),
                ColorFromHex("#E4DBFB"),
                ColorFromHex("#202534"),
                ColorFromHex("#706D86"),
                ColorFromHex("#D3CEE4"),
                ColorFromHex("#F6F3FC"),
                ColorFromHex("#FBF9FE"),
                ColorFromHex("#FFFEFF"),
                ColorFromHex("#EEE9F5"),
                ColorFromHex("#D8D2E6"),
                ColorFromHex("#FBF9FE"),
                ColorFromHex("#FFFEFF"),
                ColorFromHex("#202534"),
                ColorFromHex("#FFFFFF")),
            _ => new(
                ColorFromHex("#EDEAF2"),
                ColorFromHex("#F9F7FC"),
                ColorFromHex("#FFFFFF"),
                ColorFromHex("#F2EEF6"),
                ColorFromHex("#287F74"),
                ColorFromHex("#D8ECE8"),
                ColorFromHex("#202534"),
                ColorFromHex("#707A88"),
                ColorFromHex("#D2CFDD"),
                ColorFromHex("#F6F4FB"),
                ColorFromHex("#FAF8FD"),
                ColorFromHex("#FFFEFF"),
                ColorFromHex("#ECE8F2"),
                ColorFromHex("#D5D1DF"),
                ColorFromHex("#FAF8FD"),
                ColorFromHex("#FFFEFF"),
                ColorFromHex("#202534"),
                ColorFromHex("#FFFFFF"))
        };
    }

    private static MediaColor ColorFromHex(string hex) =>
        (MediaColor)System.Windows.Media.ColorConverter.ConvertFromString(hex)!;

    private sealed record ThemePalette(
        MediaColor WindowBackground,
        MediaColor Panel,
        MediaColor Surface,
        MediaColor SurfaceMuted,
        MediaColor Accent,
        MediaColor AccentMuted,
        MediaColor Text,
        MediaColor SubtleText,
        MediaColor Frame,
        MediaColor Screen,
        MediaColor SettingsBackground,
        MediaColor SettingsPanel,
        MediaColor ThemeChip,
        MediaColor ThemeChipBorder,
        MediaColor HeroWash,
        MediaColor HeroCard,
        MediaColor HeroOutline,
        MediaColor ButtonText);
}
