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
                ColorFromHex("#111315"),
                ColorFromHex("#171A1D"),
                ColorFromHex("#1B1F23"),
                ColorFromHex("#1F2528"),
                ColorFromHex("#65C3BA"),
                ColorFromHex("#1A2927"),
                ColorFromHex("#F4F7F6"),
                ColorFromHex("#96A3A3"),
                ColorFromHex("#141719"),
                ColorFromHex("#171B1E"),
                ColorFromHex("#1D2326"),
                ColorFromHex("#2E3A3B"),
                ColorFromHex("#1B2225"),
                ColorFromHex("#1A1F21"),
                ColorFromHex("#2A3537"),
                ColorFromHex("#F8FBFA")),
            (AppThemeMode.Dark, AppThemeVibe.Ember) => new(
                ColorFromHex("#141211"),
                ColorFromHex("#1B1715"),
                ColorFromHex("#201B18"),
                ColorFromHex("#27201C"),
                ColorFromHex("#D7946A"),
                ColorFromHex("#2A211D"),
                ColorFromHex("#FBF6F2"),
                ColorFromHex("#A8968A"),
                ColorFromHex("#151211"),
                ColorFromHex("#1A1715"),
                ColorFromHex("#241E1A"),
                ColorFromHex("#3D3028"),
                ColorFromHex("#201B18"),
                ColorFromHex("#1B1715"),
                ColorFromHex("#342923"),
                ColorFromHex("#FBF7F4")),
            (AppThemeMode.Dark, AppThemeVibe.Citrus) => new(
                ColorFromHex("#141614"),
                ColorFromHex("#191C18"),
                ColorFromHex("#1E221D"),
                ColorFromHex("#242922"),
                ColorFromHex("#A9BD64"),
                ColorFromHex("#273023"),
                ColorFromHex("#F6F8F0"),
                ColorFromHex("#9EA691"),
                ColorFromHex("#141614"),
                ColorFromHex("#181B18"),
                ColorFromHex("#20241F"),
                ColorFromHex("#32392B"),
                ColorFromHex("#1F251F"),
                ColorFromHex("#1B1F1B"),
                ColorFromHex("#2D3528"),
                ColorFromHex("#F9FBF5")),
            (AppThemeMode.Dark, AppThemeVibe.Bloom) => new(
                ColorFromHex("#141316"),
                ColorFromHex("#1A181D"),
                ColorFromHex("#201D23"),
                ColorFromHex("#27232B"),
                ColorFromHex("#C79BC8"),
                ColorFromHex("#29242E"),
                ColorFromHex("#F8F4F8"),
                ColorFromHex("#A79AA8"),
                ColorFromHex("#141316"),
                ColorFromHex("#18171B"),
                ColorFromHex("#211F24"),
                ColorFromHex("#38313A"),
                ColorFromHex("#221F26"),
                ColorFromHex("#1B191E"),
                ColorFromHex("#312B34"),
                ColorFromHex("#FBF7FB")),
            (AppThemeMode.Light, AppThemeVibe.Ember) => new(
                ColorFromHex("#F8EFE9"),
                ColorFromHex("#FBF5F0"),
                ColorFromHex("#FFFFFF"),
                ColorFromHex("#F6E6DA"),
                ColorFromHex("#D6663A"),
                ColorFromHex("#F5D9C7"),
                ColorFromHex("#2F211C"),
                ColorFromHex("#8A6C62"),
                ColorFromHex("#FFF5EF"),
                ColorFromHex("#FFF8F4"),
                ColorFromHex("#F9E7DD"),
                ColorFromHex("#E9B49A"),
                ColorFromHex("#F6D8C7"),
                ColorFromHex("#FFF7F2"),
                ColorFromHex("#F0C2AA"),
                ColorFromHex("#FFFFFF")),
            (AppThemeMode.Light, AppThemeVibe.Citrus) => new(
                ColorFromHex("#F4F6E8"),
                ColorFromHex("#FAFBF1"),
                ColorFromHex("#FFFFFF"),
                ColorFromHex("#EEF2D6"),
                ColorFromHex("#87A32B"),
                ColorFromHex("#DCE8AE"),
                ColorFromHex("#283018"),
                ColorFromHex("#768058"),
                ColorFromHex("#FBFCF3"),
                ColorFromHex("#FEFFF8"),
                ColorFromHex("#EDF3D7"),
                ColorFromHex("#D4E28E"),
                ColorFromHex("#E0EAB0"),
                ColorFromHex("#FCFEEE"),
                ColorFromHex("#C0D36E"),
                ColorFromHex("#FFFFFF")),
            (AppThemeMode.Light, AppThemeVibe.Bloom) => new(
                ColorFromHex("#F7EFF6"),
                ColorFromHex("#FCF7FB"),
                ColorFromHex("#FFFFFF"),
                ColorFromHex("#F1E0EF"),
                ColorFromHex("#B85AAE"),
                ColorFromHex("#E9C7E5"),
                ColorFromHex("#342534"),
                ColorFromHex("#8A7088"),
                ColorFromHex("#FFF8FE"),
                ColorFromHex("#FFF9FE"),
                ColorFromHex("#F3E4F1"),
                ColorFromHex("#E2BEE0"),
                ColorFromHex("#EED7EC"),
                ColorFromHex("#FFFAFE"),
                ColorFromHex("#D39BCE"),
                ColorFromHex("#FFFFFF")),
            _ => new(
                ColorFromHex("#F4F1EA"),
                ColorFromHex("#F9F6EF"),
                ColorFromHex("#FFFFFF"),
                ColorFromHex("#E8F3F1"),
                ColorFromHex("#0F766E"),
                ColorFromHex("#CBE9E4"),
                ColorFromHex("#1F2937"),
                ColorFromHex("#6B7280"),
                ColorFromHex("#F7F4EE"),
                ColorFromHex("#FCFAF5"),
                ColorFromHex("#E5F3F0"),
                ColorFromHex("#B9DDD7"),
                ColorFromHex("#D7EFEA"),
                ColorFromHex("#FFFCF8"),
                ColorFromHex("#9DD0C7"),
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
        MediaColor SettingsBackground,
        MediaColor SettingsPanel,
        MediaColor ThemeChip,
        MediaColor ThemeChipBorder,
        MediaColor HeroWash,
        MediaColor HeroCard,
        MediaColor HeroOutline,
        MediaColor ButtonText);
}
