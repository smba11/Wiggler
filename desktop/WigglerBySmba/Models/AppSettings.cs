namespace WigglerBySmba.Models;

public sealed class AppSettings
{
    public LaunchMode LaunchMode { get; set; } = LaunchMode.Window;
    public CloseBehavior CloseBehavior { get; set; } = CloseBehavior.MinimizeToTray;
    public int IdleDelaySeconds { get; set; } = 12;
    public MovementPattern Pattern { get; set; } = MovementPattern.Circle;
    public double Speed { get; set; } = 1.2;
    public double Size { get; set; } = 90;
    public ThemeMode ThemeMode { get; set; } = ThemeMode.Light;
    public ThemeVibe ThemeVibe { get; set; } = ThemeVibe.Tide;
    public bool HasCompletedOnboarding { get; set; }
}
