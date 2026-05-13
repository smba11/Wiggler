using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using WigglerBySmba.Models;
using WigglerBySmba.Services;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;
using AppThemeMode = WigglerBySmba.Models.ThemeMode;
using AppThemeVibe = WigglerBySmba.Models.ThemeVibe;

namespace WigglerBySmba.ViewModels;

public sealed class MainViewModel : ObservableObject, IDisposable
{
    private const string StartupRunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string StartupValueName = "WIGGLER by SMBA";

    private readonly AppSettings _settings;
    private readonly SettingsService _settingsService;
    private readonly MouseHookService _mouseHookService;
    private readonly MouseMovementService _mouseMovementService;
    private readonly AppLocalizationService _localizationService;
    private readonly System.Windows.Threading.DispatcherTimer _idleTimer;

    private bool _isEnabled;
    private bool _isSettingsOpen;
    private WigglerStatus _status = WigglerStatus.Off;
    private DateTime _lastUserActivityUtc = DateTime.UtcNow;
    private LaunchMode _selectedLaunchMode;
    private CloseBehavior _selectedCloseBehavior;
    private ActivationMode _selectedActivationMode;
    private MovementPattern _selectedPattern;
    private AppThemeMode _selectedThemeMode;
    private AppThemeVibe _selectedThemeVibe;
    private string _selectedLanguageCode;
    private int _idleDelaySeconds;
    private double _speed;
    private double _size;
    private bool _startOnWindowsStartup;
    private bool _rememberLastState;
    private bool _stopOnMouseMovement;
    private TakeoverSensitivity _selectedTakeoverSensitivity;
    private bool _compactMode;
    private bool _showTrayIcon;
    private bool _keepRunningInBackground;

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
        _localizationService = new AppLocalizationService();

        _selectedLaunchMode = settings.LaunchMode;
        _selectedCloseBehavior = settings.CloseBehavior;
        _selectedActivationMode = settings.ActivationMode;
        _selectedPattern = settings.Pattern;
        _selectedThemeMode = settings.ThemeMode;
        _selectedThemeVibe = settings.ThemeVibe;
        _selectedLanguageCode = string.IsNullOrWhiteSpace(settings.LanguageCode) ? "en" : settings.LanguageCode;
        _idleDelaySeconds = settings.IdleDelaySeconds;
        _speed = settings.Speed;
        _size = settings.Size;
        _startOnWindowsStartup = settings.StartOnWindowsStartup;
        _rememberLastState = settings.RememberLastState;
        _stopOnMouseMovement = settings.StopOnMouseMovement;
        _selectedTakeoverSensitivity = settings.TakeoverSensitivity;
        _compactMode = settings.CompactMode;
        _showTrayIcon = settings.ShowTrayIcon;
        _keepRunningInBackground = settings.KeepRunningInBackground;

        TogglePowerCommand = new RelayCommand(TogglePower);
        ToggleSettingsCommand = new RelayCommand(() => IsSettingsOpen = !IsSettingsOpen);
        ReplayTutorialCommand = new RelayCommand(() => RequestTutorial?.Invoke(this, EventArgs.Empty));
        ResetSettingsCommand = new RelayCommand(ResetSettings);
        AboutCommand = new RelayCommand(ShowAbout);

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
    public event EventHandler? LanguageChanged;

    public RelayCommand TogglePowerCommand { get; }
    public RelayCommand ToggleSettingsCommand { get; }
    public RelayCommand ReplayTutorialCommand { get; }
    public RelayCommand ResetSettingsCommand { get; }
    public RelayCommand AboutCommand { get; }

    public IReadOnlyList<UiOption<LaunchMode>> LaunchModeItems =>
        Enum.GetValues<LaunchMode>().Select(value => new UiOption<LaunchMode>(value, LocalizeLaunchMode(value))).ToList();

    public IReadOnlyList<UiOption<CloseBehavior>> CloseBehaviorItems =>
        Enum.GetValues<CloseBehavior>().Select(value => new UiOption<CloseBehavior>(value, LocalizeCloseBehavior(value))).ToList();

    public IReadOnlyList<UiOption<ActivationMode>> ActivationModeItems =>
        Enum.GetValues<ActivationMode>().Select(value => new UiOption<ActivationMode>(value, LocalizeActivationMode(value))).ToList();

    public IReadOnlyList<UiOption<MovementPattern>> PatternItems =>
        Enum.GetValues<MovementPattern>().Select(value => new UiOption<MovementPattern>(value, LocalizePattern(value))).ToList();

    public IReadOnlyList<UiOption<TakeoverSensitivity>> TakeoverSensitivityItems =>
        Enum.GetValues<TakeoverSensitivity>().Select(value => new UiOption<TakeoverSensitivity>(value, LocalizeTakeoverSensitivity(value))).ToList();

    public IReadOnlyList<UiOption<AppThemeMode>> ThemeModeItems =>
        Enum.GetValues<AppThemeMode>().Select(value => new UiOption<AppThemeMode>(value, LocalizeThemeMode(value))).ToList();

    public IReadOnlyList<UiOption<AppThemeVibe>> AccentColorItems =>
        Enum.GetValues<AppThemeVibe>().Select(value => new UiOption<AppThemeVibe>(value, LocalizeThemeVibe(value))).ToList();

    public IReadOnlyList<UiOption<string>> LanguageItems => _localizationService.GetLanguageOptions();

    public bool IsSettingsOpen
    {
        get => _isSettingsOpen;
        set => SetProperty(ref _isSettingsOpen, value);
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
            OnPropertyChanged(nameof(StatusDetail));
            OnPropertyChanged(nameof(StatusDotBrush));
            OnPropertyChanged(nameof(ToggleStateWord));
            OnPropertyChanged(nameof(ToggleTrackBrush));
            OnPropertyChanged(nameof(ToggleTrackBorderBrush));
            StatusChanged?.Invoke(this, value);
        }
    }

    public string BrandTitle => "WIGGLER";
    public string BrandSubtitle => "by smba";
    public string StatusLabel => Status switch
    {
        WigglerStatus.Off => Text.OffStatus,
        WigglerStatus.Armed => "Armed",
        WigglerStatus.Running => Text.RunningStatus,
        _ => Text.ReadyStatus
    };

    public string HeroLabel => Status switch
    {
        WigglerStatus.Off => Text.HeroOffLabel,
        WigglerStatus.Armed => Text.HeroReadyLabel,
        WigglerStatus.Running => Text.HeroRunningLabel,
        _ => Text.HeroReadyLabel
    };

    public string StatusDetail => Status switch
    {
        WigglerStatus.Off => Text.StatusOffDetail,
        WigglerStatus.Armed => SelectedActivationMode == ActivationMode.Immediate
            ? "Movement starts the moment you switch it on."
            : string.Format(Text.StatusReadyDetail, IdleDelayDisplay.ToLowerInvariant()),
        WigglerStatus.Running => string.Format(Text.StatusRunningDetail, LocalizedSelectedPattern),
        _ => string.Empty
    };

    public string ToggleStateWord => _isEnabled ? "ON" : "OFF";
    public bool IsPoweredOn => _isEnabled;
    public Brush ToggleTrackBrush => _isEnabled
        ? new SolidColorBrush(Color.FromRgb(18, 56, 51))
        : new SolidColorBrush(Color.FromRgb(31, 36, 44));
    public Brush ToggleTrackBorderBrush => _isEnabled
        ? new SolidColorBrush(Color.FromRgb(27, 199, 165))
        : new SolidColorBrush(Color.FromRgb(58, 67, 78));
    public Brush StatusDotBrush => Status switch
    {
        WigglerStatus.Off => new SolidColorBrush(Color.FromRgb(137, 148, 166)),
        WigglerStatus.Armed => new SolidColorBrush(Color.FromRgb(27, 199, 165)),
        WigglerStatus.Running => new SolidColorBrush(Color.FromRgb(27, 199, 165)),
        _ => new SolidColorBrush(Color.FromRgb(137, 148, 166))
    };

    public string LocalizedSelectedPattern => LocalizePattern(SelectedPattern);
    public string SpeedDisplay => $"{Speed:0.0}x";
    public string SizeDisplay => $"{Size:0}px";
    public string IdleDelayDisplay => $"{IdleDelaySeconds} sec";

    public string LaunchModeLabel => "Launch Mode";
    public string CloseBehaviorLabel => "Close Behavior";
    public string StartupLabel => "Start on Windows Startup";
    public string RememberStateLabel => "Remember Last State";
    public string PatternLabel => Text.PatternLabel;
    public string MovementSpeedLabel => "Movement Speed";
    public string MovementSizeLabel => "Movement Size";
    public string ActivationModeLabel => "Activation Mode";
    public string IdleDelayLabel => "Idle Delay";
    public string StopOnMovementLabel => "Stop on Mouse Movement";
    public string TakeoverSensitivityLabel => "Takeover Sensitivity";
    public string ThemeLabel => Text.ThemeLabel;
    public string AccentColorLabel => "Accent Color";
    public string CompactModeLabel => "Compact Mode";
    public string LanguageLabel => Text.LanguageLabel;
    public string ShowTrayIconLabel => "Show Tray Icon";
    public string KeepBackgroundLabel => "Keep Running in Background";
    public string ResetSettingsLabel => "Reset Settings";
    public string BehaviorSectionLabel => "Behavior";
    public string MovementSectionLabel => "Movement";
    public string IdleSectionLabel => "Idle";
    public string AppearanceSectionLabel => "Appearance";
    public string HelpSectionLabel => "Help";
    public string ReplayTutorialLabel => "Replay tutorial";
    public string AboutLabel => "Open WIGGLER website";

    public string FooterHint =>
        "WIGGLER runs quietly in the background and gives control back the second you move the mouse.";

    public string SelectedLanguageCode
    {
        get => _selectedLanguageCode;
        set
        {
            if (!SetProperty(ref _selectedLanguageCode, value))
            {
                return;
            }

            _settings.LanguageCode = value;
            PersistSettings();
            RefreshLocalizedProperties();
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }
    }

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

    public bool StartOnWindowsStartup
    {
        get => _startOnWindowsStartup;
        set
        {
            if (!SetProperty(ref _startOnWindowsStartup, value))
            {
                return;
            }

            _settings.StartOnWindowsStartup = value;
            ApplyStartupSetting(value);
            PersistSettings();
        }
    }

    public bool RememberLastState
    {
        get => _rememberLastState;
        set
        {
            if (!SetProperty(ref _rememberLastState, value))
            {
                return;
            }

            _settings.RememberLastState = value;
            if (!value)
            {
                _settings.LastEnabledState = false;
            }

            PersistSettings();
        }
    }

    public ActivationMode SelectedActivationMode
    {
        get => _selectedActivationMode;
        set
        {
            if (!SetProperty(ref _selectedActivationMode, value))
            {
                return;
            }

            _settings.ActivationMode = value;
            PersistSettings();
            OnPropertyChanged(nameof(StatusDetail));
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
            PersistSettings();
            OnPropertyChanged(nameof(IdleDelayDisplay));
            OnPropertyChanged(nameof(StatusDetail));
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
            PersistSettings();
            OnPropertyChanged(nameof(LocalizedSelectedPattern));
            OnPropertyChanged(nameof(StatusDetail));

            if (Status == WigglerStatus.Running)
            {
                RestartMovement();
            }
        }
    }

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
            PersistSettings();
            OnPropertyChanged(nameof(SpeedDisplay));

            if (Status == WigglerStatus.Running)
            {
                RestartMovement();
            }
        }
    }

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
            PersistSettings();
            OnPropertyChanged(nameof(SizeDisplay));

            if (Status == WigglerStatus.Running)
            {
                RestartMovement();
            }
        }
    }

    public bool StopOnMouseMovement
    {
        get => _stopOnMouseMovement;
        set
        {
            if (!SetProperty(ref _stopOnMouseMovement, value))
            {
                return;
            }

            _settings.StopOnMouseMovement = value;
            PersistSettings();
        }
    }

    public TakeoverSensitivity SelectedTakeoverSensitivity
    {
        get => _selectedTakeoverSensitivity;
        set
        {
            if (!SetProperty(ref _selectedTakeoverSensitivity, value))
            {
                return;
            }

            _settings.TakeoverSensitivity = value;
            PersistSettings();
        }
    }

    public AppThemeMode SelectedThemeMode
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

    public AppThemeVibe SelectedThemeVibe
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

    public bool CompactMode
    {
        get => _compactMode;
        set
        {
            if (!SetProperty(ref _compactMode, value))
            {
                return;
            }

            _settings.CompactMode = value;
            PersistSettings();
        }
    }

    public bool ShowTrayIcon
    {
        get => _showTrayIcon;
        set
        {
            if (!SetProperty(ref _showTrayIcon, value))
            {
                return;
            }

            _settings.ShowTrayIcon = value;
            PersistSettings();
        }
    }

    public bool KeepRunningInBackground
    {
        get => _keepRunningInBackground;
        set
        {
            if (!SetProperty(ref _keepRunningInBackground, value))
            {
                return;
            }

            _settings.KeepRunningInBackground = value;
            PersistSettings();
        }
    }

    public void Initialize()
    {
        ThemeChanged?.Invoke(this, EventArgs.Empty);

        if (!_settings.HasCompletedOnboarding)
        {
            RequestTutorial?.Invoke(this, EventArgs.Empty);
            return;
        }

        IsSettingsOpen = false;

        if (_settings.RememberLastState && _settings.LastEnabledState)
        {
            _isEnabled = true;
            _lastUserActivityUtc = DateTime.UtcNow;
            ApplyExecutionState();
            if (SelectedActivationMode == ActivationMode.Immediate)
            {
                StartMovementNow();
            }
            else
            {
                Status = WigglerStatus.Armed;
            }
            OnPropertyChanged(nameof(ToggleStateWord));
            OnPropertyChanged(nameof(IsPoweredOn));
            OnPropertyChanged(nameof(ToggleTrackBrush));
            OnPropertyChanged(nameof(ToggleTrackBorderBrush));
        }
    }

    public void CompleteOnboarding()
    {
        if (_settings.HasCompletedOnboarding)
        {
            return;
        }

        _settings.HasCompletedOnboarding = true;
        PersistSettings();
        IsSettingsOpen = false;
    }

    public IReadOnlyList<AppLocalizationService.TutorialPageText> GetTutorialPages() =>
        Text.TutorialPages ?? Array.Empty<AppLocalizationService.TutorialPageText>();

    private AppLocalizationService.AppTextPack Text => _localizationService.GetPack(SelectedLanguageCode);

    private void TogglePower()
    {
        _isEnabled = !_isEnabled;
        _settings.LastEnabledState = RememberLastState && _isEnabled;

        if (_isEnabled)
        {
            _lastUserActivityUtc = DateTime.UtcNow;
            ApplyExecutionState();
            if (SelectedActivationMode == ActivationMode.Immediate)
            {
                StartMovementNow();
            }
            else
            {
                Status = WigglerStatus.Armed;
            }
        }
        else
        {
            StopMovement();
            ReleaseExecutionState();
            Status = WigglerStatus.Off;
        }

        PersistSettings();
        OnPropertyChanged(nameof(ToggleStateWord));
        OnPropertyChanged(nameof(IsPoweredOn));
        OnPropertyChanged(nameof(ToggleTrackBrush));
        OnPropertyChanged(nameof(ToggleTrackBorderBrush));
    }

    private void OnUserMouseActivity(object? sender, EventArgs e)
    {
        _lastUserActivityUtc = DateTime.UtcNow;
        if (_isEnabled && StopOnMouseMovement && Status == WigglerStatus.Running)
        {
            StopMovement();
            Status = WigglerStatus.Armed;
        }
    }

    private void OnIdleTick(object? sender, EventArgs e)
    {
        if (!_isEnabled || SelectedActivationMode == ActivationMode.Immediate)
        {
            return;
        }

        if (Status == WigglerStatus.Armed && DateTime.UtcNow - _lastUserActivityUtc >= TimeSpan.FromSeconds(IdleDelaySeconds))
        {
            StartMovementNow();
        }
    }

    private void StartMovementNow()
    {
        _mouseMovementService.Start(SelectedPattern, Speed, Size);
        Status = WigglerStatus.Running;
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

    private void ResetSettings()
    {
        var reset = new AppSettings
        {
            HasCompletedOnboarding = _settings.HasCompletedOnboarding
        };

        _settings.LanguageCode = reset.LanguageCode;
        _settings.LaunchMode = reset.LaunchMode;
        _settings.CloseBehavior = reset.CloseBehavior;
        _settings.StartOnWindowsStartup = reset.StartOnWindowsStartup;
        _settings.RememberLastState = reset.RememberLastState;
        _settings.LastEnabledState = false;
        _settings.IdleDelaySeconds = reset.IdleDelaySeconds;
        _settings.ActivationMode = reset.ActivationMode;
        _settings.Pattern = reset.Pattern;
        _settings.Speed = reset.Speed;
        _settings.Size = reset.Size;
        _settings.StopOnMouseMovement = reset.StopOnMouseMovement;
        _settings.TakeoverSensitivity = reset.TakeoverSensitivity;
        _settings.ThemeMode = reset.ThemeMode;
        _settings.ThemeVibe = reset.ThemeVibe;
        _settings.CompactMode = reset.CompactMode;
        _settings.ShowTrayIcon = reset.ShowTrayIcon;
        _settings.KeepRunningInBackground = reset.KeepRunningInBackground;

        _selectedLanguageCode = _settings.LanguageCode;
        _selectedLaunchMode = _settings.LaunchMode;
        _selectedCloseBehavior = _settings.CloseBehavior;
        _startOnWindowsStartup = _settings.StartOnWindowsStartup;
        _rememberLastState = _settings.RememberLastState;
        _idleDelaySeconds = _settings.IdleDelaySeconds;
        _selectedActivationMode = _settings.ActivationMode;
        _selectedPattern = _settings.Pattern;
        _speed = _settings.Speed;
        _size = _settings.Size;
        _stopOnMouseMovement = _settings.StopOnMouseMovement;
        _selectedTakeoverSensitivity = _settings.TakeoverSensitivity;
        _selectedThemeMode = _settings.ThemeMode;
        _selectedThemeVibe = _settings.ThemeVibe;
        _compactMode = _settings.CompactMode;
        _showTrayIcon = _settings.ShowTrayIcon;
        _keepRunningInBackground = _settings.KeepRunningInBackground;

        ApplyStartupSetting(false);
        PersistSettings();
        RefreshLocalizedProperties();
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ShowAbout()
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://smba11.github.io/Wiggler/",
                UseShellExecute = true
            });
        }
        catch
        {
            // Best effort only.
        }
    }

    private void ApplyStartupSetting(bool enabled)
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(StartupRunKey, true) ?? Registry.CurrentUser.CreateSubKey(StartupRunKey);
            if (key is null)
            {
                return;
            }

            if (!enabled)
            {
                key.DeleteValue(StartupValueName, false);
                return;
            }

            var executablePath = Process.GetCurrentProcess().MainModule?.FileName;
            if (!string.IsNullOrWhiteSpace(executablePath))
            {
                key.SetValue(StartupValueName, $"\"{executablePath}\"");
            }
        }
        catch
        {
            // Best effort only; failing here should not break the app.
        }
    }

    private string LocalizePattern(MovementPattern pattern) => pattern switch
    {
        MovementPattern.Circle => Text.GetPatternName("Circle"),
        MovementPattern.Square => Text.GetPatternName("Square"),
        MovementPattern.Triangle => Text.GetPatternName("Triangle"),
        MovementPattern.Figure8 => Text.GetPatternName("Figure 8"),
        MovementPattern.Parallelogram => Text.GetPatternName("Parallelogram"),
        MovementPattern.Random => Text.GetPatternName("Random"),
        _ => pattern.ToString()
    };

    private string LocalizeLaunchMode(LaunchMode mode) => mode switch
    {
        LaunchMode.Window => SelectedLanguageCode switch
        {
            "es" => "Ventana",
            "pt" => "Janela",
            "fr" => "Fenêtre",
            "de" => "Fenster",
            "it" => "Finestra",
            "nl" => "Venster",
            "sv" => "Fönster",
            "ja" => "ウィンドウ",
            "ko" => "창",
            "zh" => "窗口",
            "ar" => "نافذة",
            "hi" => "विंडो",
            _ => "Window"
        },
        LaunchMode.Tray => "Tray",
        _ => mode.ToString()
    };

    private string LocalizeCloseBehavior(CloseBehavior value) => value switch
    {
        CloseBehavior.MinimizeToTray => SelectedLanguageCode switch
        {
            "es" => "Minimizar a bandeja",
            "pt" => "Minimizar para a bandeja",
            "fr" => "Réduire dans le tray",
            "de" => "Ins Tray minimieren",
            "it" => "Riduci nel tray",
            "nl" => "Minimaliseren naar tray",
            "sv" => "Minimera till tray",
            "ja" => "トレイに最小化",
            "ko" => "트레이로 최소화",
            "zh" => "最小化到托盘",
            "ar" => "تصغير إلى الدرج",
            "hi" => "ट्रे में मिनिमाइज़",
            _ => "Minimize to tray"
        },
        CloseBehavior.Exit => SelectedLanguageCode switch
        {
            "es" => "Salir",
            "pt" => "Sair",
            "fr" => "Quitter",
            "de" => "Beenden",
            "it" => "Esci",
            "nl" => "Afsluiten",
            "sv" => "Avsluta",
            "ja" => "終了",
            "ko" => "종료",
            "zh" => "退出",
            "ar" => "خروج",
            "hi" => "बंद करें",
            _ => "Exit"
        },
        _ => value.ToString()
    };

    private string LocalizeActivationMode(ActivationMode value) => value switch
    {
        ActivationMode.AfterIdle => "After idle",
        ActivationMode.Immediate => "Immediate",
        _ => value.ToString()
    };

    private string LocalizeTakeoverSensitivity(TakeoverSensitivity value) => value switch
    {
        TakeoverSensitivity.Low => "Low",
        TakeoverSensitivity.Normal => "Normal",
        TakeoverSensitivity.High => "High",
        _ => value.ToString()
    };

    private string LocalizeThemeMode(AppThemeMode value) => value switch
    {
        AppThemeMode.Light => SelectedLanguageCode switch
        {
            "es" => "Claro",
            "pt" => "Claro",
            "fr" => "Clair",
            "de" => "Hell",
            "it" => "Chiaro",
            "nl" => "Licht",
            "sv" => "Ljust",
            "ja" => "ライト",
            "ko" => "라이트",
            "zh" => "浅色",
            "ar" => "فاتح",
            "hi" => "लाइट",
            _ => "Light"
        },
        AppThemeMode.Dark => SelectedLanguageCode switch
        {
            "es" => "Oscuro",
            "pt" => "Escuro",
            "fr" => "Sombre",
            "de" => "Dunkel",
            "it" => "Scuro",
            "nl" => "Donker",
            "sv" => "Mörkt",
            "ja" => "ダーク",
            "ko" => "다크",
            "zh" => "深色",
            "ar" => "داكن",
            "hi" => "डार्क",
            _ => "Dark"
        },
        _ => value.ToString()
    };

    private string LocalizeThemeVibe(AppThemeVibe value) => value switch
    {
        AppThemeVibe.Tide => "Tide",
        AppThemeVibe.Ember => "Ember",
        AppThemeVibe.Citrus => "Citrus",
        AppThemeVibe.Bloom => "Bloom",
        _ => value.ToString()
    };

    private void RefreshLocalizedProperties()
    {
        foreach (var property in new[]
        {
            nameof(StatusLabel), nameof(HeroLabel), nameof(StatusDetail), nameof(LocalizedSelectedPattern),
            nameof(PatternLabel), nameof(ThemeLabel), nameof(LanguageLabel),
            nameof(LaunchModeItems), nameof(CloseBehaviorItems), nameof(ActivationModeItems), nameof(PatternItems),
            nameof(TakeoverSensitivityItems), nameof(ThemeModeItems), nameof(AccentColorItems), nameof(LanguageItems)
        })
        {
            OnPropertyChanged(property);
        }
    }

    private void PersistSettings() => _settingsService.Save(_settings);

    private static void ApplyExecutionState()
    {
        NativeMethods.SetThreadExecutionState(
            NativeMethods.EsContinuous |
            NativeMethods.EsDisplayRequired |
            NativeMethods.EsSystemRequired);
    }

    private static void ReleaseExecutionState()
    {
        NativeMethods.SetThreadExecutionState(NativeMethods.EsContinuous);
    }

    public void Dispose()
    {
        ReleaseExecutionState();
        _idleTimer.Stop();
        _idleTimer.Tick -= OnIdleTick;
        _mouseHookService.UserMouseActivity -= OnUserMouseActivity;
        _mouseMovementService.Dispose();
        _mouseHookService.Dispose();
    }
}

internal static class AppTextPackExtensions
{
    public static string GetPatternName(this AppLocalizationService.AppTextPack pack, string pattern) => pack.LanguageName switch
    {
        "Español" => pattern switch
        {
            "Circle" => "Círculo",
            "Square" => "Cuadrado",
            "Triangle" => "Triángulo",
            "Figure 8" => "Figura 8",
            "Parallelogram" => "Paralelogramo",
            "Random" => "Aleatorio",
            _ => pattern
        },
        "Português" => pattern switch
        {
            "Circle" => "Círculo",
            "Square" => "Quadrado",
            "Triangle" => "Triângulo",
            "Figure 8" => "Figura 8",
            "Parallelogram" => "Paralelogramo",
            "Random" => "Aleatório",
            _ => pattern
        },
        "Français" => pattern switch
        {
            "Circle" => "Cercle",
            "Square" => "Carré",
            "Triangle" => "Triangle",
            "Figure 8" => "Figure 8",
            "Parallelogram" => "Parallélogramme",
            "Random" => "Aléatoire",
            _ => pattern
        },
        "Deutsch" => pattern switch
        {
            "Circle" => "Kreis",
            "Square" => "Quadrat",
            "Triangle" => "Dreieck",
            "Figure 8" => "Acht",
            "Parallelogram" => "Parallelogramm",
            "Random" => "Zufall",
            _ => pattern
        },
        "Italiano" => pattern switch
        {
            "Circle" => "Cerchio",
            "Square" => "Quadrato",
            "Triangle" => "Triangolo",
            "Figure 8" => "Figura 8",
            "Parallelogram" => "Parallelogramma",
            "Random" => "Casuale",
            _ => pattern
        },
        "Nederlands" => pattern switch
        {
            "Circle" => "Cirkel",
            "Square" => "Vierkant",
            "Triangle" => "Driehoek",
            "Figure 8" => "Figuur 8",
            "Parallelogram" => "Parallellogram",
            "Random" => "Willekeurig",
            _ => pattern
        },
        "Svenska" => pattern switch
        {
            "Circle" => "Cirkel",
            "Square" => "Kvadrat",
            "Triangle" => "Triangel",
            "Figure 8" => "Figur 8",
            "Parallelogram" => "Parallellogram",
            "Random" => "Slumpmässig",
            _ => pattern
        },
        "日本語" => pattern switch
        {
            "Circle" => "円",
            "Square" => "四角",
            "Triangle" => "三角",
            "Figure 8" => "8の字",
            "Parallelogram" => "平行四辺形",
            "Random" => "ランダム",
            _ => pattern
        },
        "한국어" => pattern switch
        {
            "Circle" => "원",
            "Square" => "사각형",
            "Triangle" => "삼각형",
            "Figure 8" => "8자",
            "Parallelogram" => "평행사변형",
            "Random" => "랜덤",
            _ => pattern
        },
        "中文" => pattern switch
        {
            "Circle" => "圆形",
            "Square" => "方形",
            "Triangle" => "三角形",
            "Figure 8" => "8字形",
            "Parallelogram" => "平行四边形",
            "Random" => "随机",
            _ => pattern
        },
        "العربية" => pattern switch
        {
            "Circle" => "دائرة",
            "Square" => "مربع",
            "Triangle" => "مثلث",
            "Figure 8" => "شكل 8",
            "Parallelogram" => "متوازي أضلاع",
            "Random" => "عشوائي",
            _ => pattern
        },
        "हिन्दी" => pattern switch
        {
            "Circle" => "वृत्त",
            "Square" => "वर्ग",
            "Triangle" => "त्रिभुज",
            "Figure 8" => "आठ आकृति",
            "Parallelogram" => "समांतर चतुर्भुज",
            "Random" => "रैंडम",
            _ => pattern
        },
        _ => pattern
    };
}
