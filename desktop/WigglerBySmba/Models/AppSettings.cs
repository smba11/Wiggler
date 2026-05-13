namespace WigglerBySmba.Models;

public sealed class AppSettings
{
    public string LanguageCode { get; set; } = "en";
    public LaunchMode LaunchMode { get; set; } = LaunchMode.Window;
    public CloseBehavior CloseBehavior { get; set; } = CloseBehavior.MinimizeToTray;
    public bool StartOnWindowsStartup { get; set; }
    public bool RememberLastState { get; set; } = true;
    public bool LastEnabledState { get; set; }
    public int IdleDelaySeconds { get; set; } = 12;
    public ActivationMode ActivationMode { get; set; } = ActivationMode.AfterIdle;
    public MovementPattern Pattern { get; set; } = MovementPattern.Circle;
    public double Speed { get; set; } = 1.2;
    public double Size { get; set; } = 90;
    public bool StopOnMouseMovement { get; set; } = true;
    public TakeoverSensitivity TakeoverSensitivity { get; set; } = TakeoverSensitivity.Normal;
    public ThemeMode ThemeMode { get; set; } = ThemeMode.Dark;
    public ThemeVibe ThemeVibe { get; set; } = ThemeVibe.Tide;
    public bool CompactMode { get; set; }
    public bool ShowTrayIcon { get; set; } = true;
    public bool KeepRunningInBackground { get; set; } = true;
    public bool HasCompletedOnboarding { get; set; }
}
