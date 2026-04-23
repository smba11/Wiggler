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
                ColorFromHex("#0B1418"),
                ColorFromHex("#101F25"),
                ColorFromHex("#132A32"),
                ColorFromHex("#173540"),
                ColorFromHex("#5ED8C7"),
                ColorFromHex("#173A37"),
                ColorFromHex("#F3FBF9"),
                ColorFromHex("#A8C2BE"),
                ColorFromHex("#0A1115"),
                ColorFromHex("#0F1A20"),
                ColorFromHex("#16313A"),
                ColorFromHex("#29515A"),
                ColorFromHex("#11323B"),
                ColorFromHex("#0F2026"),
                ColorFromHex("#245663"),
                ColorFromHex("#081011")),
            (AppThemeMode.Dark, AppThemeVibe.Ember) => new(
                ColorFromHex("#140E0C"),
                ColorFromHex("#201311"),
                ColorFromHex("#2B1916"),
                ColorFromHex("#38201B"),
                ColorFromHex("#FF9D6E"),
                ColorFromHex("#46261E"),
                ColorFromHex("#FFF4EF"),
                ColorFromHex("#D7B3A5"),
                ColorFromHex("#100A09"),
                ColorFromHex("#180F0D"),
                ColorFromHex("#39211C"),
                ColorFromHex("#704030"),
                ColorFromHex("#2F1D18"),
                ColorFromHex("#1D1210"),
                ColorFromHex("#5C3024"),
                ColorFromHex("#0F0908")),
            (AppThemeMode.Dark, AppThemeVibe.Citrus) => new(
                ColorFromHex("#11130D"),
                ColorFromHex("#181D12"),
                ColorFromHex("#222916"),
                ColorFromHex("#2F381D"),
                ColorFromHex("#DDF06E"),
                ColorFromHex("#3C481A"),
                ColorFromHex("#FBFEEA"),
                ColorFromHex("#C0C79A"),
                ColorFromHex("#0D0F0A"),
                ColorFromHex("#151A10"),
                ColorFromHex("#2A341A"),
                ColorFromHex("#566822"),
                ColorFromHex("#27301A"),
                ColorFromHex("#171C12"),
                ColorFromHex("#495922"),
                ColorFromHex("#0A0C08")),
            (AppThemeMode.Dark, AppThemeVibe.Bloom) => new(
                ColorFromHex("#110D15"),
                ColorFromHex("#1A1320"),
                ColorFromHex("#24192D"),
                ColorFromHex("#301F3D"),
                ColorFromHex("#F0A6E1"),
                ColorFromHex("#402447"),
                ColorFromHex("#FFF4FD"),
                ColorFromHex("#D2B3CF"),
                ColorFromHex("#0D0910"),
                ColorFromHex("#16101B"),
                ColorFromHex("#2B1C34"),
                ColorFromHex("#613A67"),
                ColorFromHex("#281A31"),
                ColorFromHex("#18111D"),
                ColorFromHex("#4A2E52"),
                ColorFromHex("#09070B")),
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
