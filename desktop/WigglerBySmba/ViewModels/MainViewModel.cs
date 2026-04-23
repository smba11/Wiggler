using System.Windows.Media;
using WigglerBySmba.Models;
using WigglerBySmba.Services;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace WigglerBySmba.ViewModels;

public sealed class MainViewModel : ObservableObject, IDisposable
{
    private readonly AppSettings _settings;
    private readonly SettingsService _settingsService;
    private readonly MouseHookService _mouseHookService;
    private readonly MouseMovementService _mouseMovementService;
    private readonly System.Windows.Threading.DispatcherTimer _idleTimer;

    private bool _isEnabled;
    private bool _isSettingsOpen;
    private WigglerStatus _status = WigglerStatus.Off;
    private DateTime _lastUserActivityUtc = DateTime.UtcNow;
    private LaunchMode _selectedLaunchMode;
    private CloseBehavior _selectedCloseBehavior;
    private MovementPattern _selectedPattern;
    private ThemeMode _selectedThemeMode;
    private ThemeVibe _selectedThemeVibe;
    private int _idleDelaySeconds;
    private double _speed;
    private double _size;

    public MainViewModel(
        AppSettings settings,
        SettingsService settingsService,
        MouseHookService mouseHookService,
        MouseMovementService mouseMovementService)
    {
        _settings = settings;
        _settingsService = settingsService;
        _mouseHookService = mouseHookService;
        _mouseMovementService = mouseMovementService;

        _selectedLaunchMode = settings.LaunchMode;
        _selectedCloseBehavior = settings.CloseBehavior;
        _selectedPattern = settings.Pattern;
        _selectedThemeMode = settings.ThemeMode;
        _selectedThemeVibe = settings.ThemeVibe;
        _idleDelaySeconds = settings.IdleDelaySeconds;
        _speed = settings.Speed;
        _size = settings.Size;

        TogglePowerCommand = new RelayCommand(TogglePower);
        ToggleSettingsCommand = new RelayCommand(() => IsSettingsOpen = !IsSettingsOpen);
        ReplayTutorialCommand = new RelayCommand(() => RequestTutorial?.Invoke(this, EventArgs.Empty));

        _mouseHookService.UserMouseActivity += OnUserMouseActivity;

        _idleTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(250)
        };
        _idleTimer.Tick += OnIdleTick;
        _idleTimer.Start();
    }

    public event EventHandler? RequestTutorial;
    public event EventHandler<WigglerStatus>? StatusChanged;
    public event EventHandler? ThemeChanged;

    public RelayCommand TogglePowerCommand { get; }
    public RelayCommand ToggleSettingsCommand { get; }
    public RelayCommand ReplayTutorialCommand { get; }

    public Array LaunchModeOptions => Enum.GetValues<LaunchMode>();
    public Array CloseBehaviorOptions => Enum.GetValues<CloseBehavior>();
    public Array PatternOptions => Enum.GetValues<MovementPattern>();
    public Array ThemeModeOptions => Enum.GetValues<ThemeMode>();
    public Array ThemeVibeOptions => Enum.GetValues<ThemeVibe>();

    public bool ShouldLaunchInTray => SelectedLaunchMode == LaunchMode.Tray && _settings.HasCompletedOnboarding;

    public bool IsSettingsOpen
    {
        get => _isSettingsOpen;
        set
        {
            if (!SetProperty(ref _isSettingsOpen, value))
            {
                return;
            }

            OnPropertyChanged(nameof(SettingsButtonLabel));
        }
    }

    public WigglerStatus Status
    {
        get => _status;
        private set
        {
            if (!SetProperty(ref _status, value))
            {
                return;
            }

            OnPropertyChanged(nameof(StatusLabel));
            OnPropertyChanged(nameof(HeroLabel));
            OnPropertyChanged(nameof(BehaviorSummary));
            OnPropertyChanged(nameof(StatusDetail));
            OnPropertyChanged(nameof(ToggleButtonLabel));
            OnPropertyChanged(nameof(ToggleButtonBrush));
            StatusChanged?.Invoke(this, value);
        }
    }

    public string StatusLabel => Status.ToString();

    public string HeroLabel => Status switch
    {
        WigglerStatus.Off => "Off until you say otherwise.",
        WigglerStatus.Armed => "Armed and waiting for idle.",
        WigglerStatus.Running => "Gliding until you take back control.",
        _ => "Ready."
    };

    public string BehaviorSummary => Status switch
    {
        WigglerStatus.Off => "WIGGLER is off.",
        WigglerStatus.Armed => $"Armed. It starts after {IdleDelayDisplay.ToLowerInvariant()} of inactivity.",
        WigglerStatus.Running => "Active. Movement will stop the instant you touch the mouse.",
        _ => string.Empty
    };

    public string PatternSummary => $"{SelectedPattern} pattern, {SpeedDisplay.ToLowerInvariant()}, {SizeDisplay.ToLowerInvariant()}";

    public string StatusDetail => Status switch
    {
        WigglerStatus.Off => "Idle monitoring is paused.",
        WigglerStatus.Armed => $"Watching for {IdleDelayDisplay.ToLowerInvariant()} before starting movement.",
        WigglerStatus.Running => $"Running a smooth {SelectedPattern} path now. Real mouse input cuts in immediately.",
        _ => string.Empty
    };

    public string ToggleButtonLabel => _isEnabled ? "Turn OFF" : "Turn ON";

    public Brush ToggleButtonBrush => _isEnabled
        ? new SolidColorBrush(Color.FromRgb(153, 27, 27))
        : new SolidColorBrush(Color.FromRgb(15, 118, 110));

    public string SettingsButtonLabel => IsSettingsOpen ? "Hide settings" : "Open settings";

    public LaunchMode SelectedLaunchMode
    {
        get => _selectedLaunchMode;
        set
        {
            if (!SetProperty(ref _selectedLaunchMode, value))
            {
                return;
            }

            _settings.LaunchMode = value;
            PersistSettings();
        }
    }

    public CloseBehavior SelectedCloseBehavior
    {
        get => _selectedCloseBehavior;
        set
        {
            if (!SetProperty(ref _selectedCloseBehavior, value))
            {
                return;
            }

            _settings.CloseBehavior = value;
            PersistSettings();
        }
    }

    public ThemeMode SelectedThemeMode
    {
        get => _selectedThemeMode;
        set
        {
            if (!SetProperty(ref _selectedThemeMode, value))
            {
                return;
            }

            _settings.ThemeMode = value;
            PersistSettings();
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public ThemeVibe SelectedThemeVibe
    {
        get => _selectedThemeVibe;
        set
        {
            if (!SetProperty(ref _selectedThemeVibe, value))
            {
                return;
            }

            _settings.ThemeVibe = value;
            PersistSettings();
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public MovementPattern SelectedPattern
    {
        get => _selectedPattern;
        set
        {
            if (!SetProperty(ref _selectedPattern, value))
            {
                return;
            }

            _settings.Pattern = value;
            OnPropertyChanged(nameof(PatternSummary));
            OnPropertyChanged(nameof(StatusDetail));
            PersistSettings();

            if (Status == WigglerStatus.Running)
            {
                RestartMovement();
            }
        }
    }

    public int IdleDelaySeconds
    {
        get => _idleDelaySeconds;
        set
        {
            if (!SetProperty(ref _idleDelaySeconds, value))
            {
                return;
            }

            _settings.IdleDelaySeconds = value;
            OnPropertyChanged(nameof(IdleDelayDisplay));
            OnPropertyChanged(nameof(BehaviorSummary));
            OnPropertyChanged(nameof(StatusDetail));
            PersistSettings();
        }
    }

    public string IdleDelayDisplay => $"{IdleDelaySeconds} seconds";

    public double Speed
    {
        get => _speed;
        set
        {
            if (!SetProperty(ref _speed, value))
            {
                return;
            }

            _settings.Speed = value;
            OnPropertyChanged(nameof(SpeedDisplay));
            OnPropertyChanged(nameof(PatternSummary));
            PersistSettings();

            if (Status == WigglerStatus.Running)
            {
                RestartMovement();
            }
        }
    }

    public string SpeedDisplay => $"{Speed:0.0}x speed";

    public double Size
    {
        get => _size;
        set
        {
            if (!SetProperty(ref _size, value))
            {
                return;
            }

            _settings.Size = value;
            OnPropertyChanged(nameof(SizeDisplay));
            OnPropertyChanged(nameof(PatternSummary));
            PersistSettings();

            if (Status == WigglerStatus.Running)
            {
                RestartMovement();
            }
        }
    }

    public string SizeDisplay => $"{Size:0}px range";

    public void Initialize()
    {
        ThemeChanged?.Invoke(this, EventArgs.Empty);

        if (!_settings.HasCompletedOnboarding)
        {
            RequestTutorial?.Invoke(this, EventArgs.Empty);
            return;
        }

        IsSettingsOpen = true;
    }

    public void CompleteOnboarding()
    {
        if (_settings.HasCompletedOnboarding)
        {
            return;
        }

        _settings.HasCompletedOnboarding = true;
        PersistSettings();
        IsSettingsOpen = true;
    }

    private void TogglePower()
    {
        _isEnabled = !_isEnabled;
        if (_isEnabled)
        {
            _lastUserActivityUtc = DateTime.UtcNow;
            Status = WigglerStatus.Armed;
        }
        else
        {
            StopMovement();
            Status = WigglerStatus.Off;
        }
    }

    private void OnUserMouseActivity(object? sender, EventArgs e)
    {
        _lastUserActivityUtc = DateTime.UtcNow;
        if (_isEnabled && Status == WigglerStatus.Running)
        {
            StopMovement();
            Status = WigglerStatus.Armed;
        }
    }

    private void OnIdleTick(object? sender, EventArgs e)
    {
        if (!_isEnabled)
        {
            return;
        }

        if (Status == WigglerStatus.Armed && DateTime.UtcNow - _lastUserActivityUtc >= TimeSpan.FromSeconds(IdleDelaySeconds))
        {
            _mouseMovementService.Start(SelectedPattern, Speed, Size);
            Status = WigglerStatus.Running;
        }
    }

    private void RestartMovement()
    {
        _mouseMovementService.Stop();
        _mouseMovementService.Start(SelectedPattern, Speed, Size);
    }

    private void StopMovement()
    {
        if (_mouseMovementService.IsRunning)
        {
            _mouseMovementService.Stop();
        }
    }

    private void PersistSettings() => _settingsService.Save(_settings);

    public void Dispose()
    {
        _idleTimer.Stop();
        _idleTimer.Tick -= OnIdleTick;
        _mouseHookService.UserMouseActivity -= OnUserMouseActivity;
        _mouseMovementService.Dispose();
        _mouseHookService.Dispose();
    }
}
