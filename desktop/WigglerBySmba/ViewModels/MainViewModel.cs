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
    private readonly AppLocalizationService _localizationService;
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
    private string _selectedLanguageCode;
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
        _localizationService = new AppLocalizationService();

        _selectedLaunchMode = settings.LaunchMode;
        _selectedCloseBehavior = settings.CloseBehavior;
        _selectedPattern = settings.Pattern;
        _selectedThemeMode = settings.ThemeMode;
        _selectedThemeVibe = settings.ThemeVibe;
        _selectedLanguageCode = string.IsNullOrWhiteSpace(settings.LanguageCode) ? "en" : settings.LanguageCode;
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
    public event EventHandler? LanguageChanged;

    public RelayCommand TogglePowerCommand { get; }
    public RelayCommand ToggleSettingsCommand { get; }
    public RelayCommand ReplayTutorialCommand { get; }

    public IReadOnlyList<UiOption<LaunchMode>> LaunchModeItems =>
        Enum.GetValues<LaunchMode>().Select(value => new UiOption<LaunchMode>(value, LocalizeLaunchMode(value))).ToList();

    public IReadOnlyList<UiOption<CloseBehavior>> CloseBehaviorItems =>
        Enum.GetValues<CloseBehavior>().Select(value => new UiOption<CloseBehavior>(value, LocalizeCloseBehavior(value))).ToList();

    public IReadOnlyList<UiOption<MovementPattern>> PatternItems =>
        Enum.GetValues<MovementPattern>().Select(value => new UiOption<MovementPattern>(value, LocalizePattern(value))).ToList();

    public IReadOnlyList<UiOption<ThemeMode>> ThemeModeItems =>
        Enum.GetValues<ThemeMode>().Select(value => new UiOption<ThemeMode>(value, LocalizeThemeMode(value))).ToList();

    public IReadOnlyList<UiOption<ThemeVibe>> ThemeVibeItems =>
        Enum.GetValues<ThemeVibe>().Select(value => new UiOption<ThemeVibe>(value, LocalizeThemeVibe(value))).ToList();

    public IReadOnlyList<UiOption<string>> LanguageItems => _localizationService.GetLanguageOptions();

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

    public string StatusLabel => Status switch
    {
        WigglerStatus.Off => Text.OffStatus,
        WigglerStatus.Armed => Text.ReadyStatus,
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

    public string BehaviorSummary => Status switch
    {
        WigglerStatus.Off => Text.BehaviorOffSummary,
        WigglerStatus.Armed => string.Format(Text.BehaviorReadySummary, IdleDelayDisplay.ToLowerInvariant()),
        WigglerStatus.Running => Text.BehaviorRunningSummary,
        _ => string.Empty
    };

    public string PatternSummary =>
        $"{LocalizedSelectedPattern} {Text.PatternLabel.ToLowerInvariant()}, {SpeedDisplay.ToLowerInvariant()}, {SizeDisplay.ToLowerInvariant()}";

    public string StatusDetail => Status switch
    {
        WigglerStatus.Off => Text.StatusOffDetail,
        WigglerStatus.Armed => string.Format(Text.StatusReadyDetail, IdleDelayDisplay.ToLowerInvariant()),
        WigglerStatus.Running => string.Format(Text.StatusRunningDetail, LocalizedSelectedPattern),
        _ => string.Empty
    };

    public string ToggleButtonLabel => _isEnabled ? Text.ToggleOffLabel : Text.ToggleOnLabel;

    public Brush ToggleButtonBrush => _isEnabled
        ? new SolidColorBrush(Color.FromRgb(153, 27, 27))
        : new SolidColorBrush(Color.FromRgb(15, 118, 110));

    public string SettingsButtonLabel => IsSettingsOpen ? Text.HideSettingsLabel : Text.OpenSettingsLabel;
    public string LocalizedSelectedPattern => LocalizePattern(SelectedPattern);
    public string SpeedDisplay => $"{Speed:0.0}x";
    public string SizeDisplay => $"{Size:0}px";
    public string IdleDelayDisplay => $"{IdleDelaySeconds} sec";

    public string BrandTitle => Text.BrandTitle;
    public string BrandSubtitle => Text.BrandSubtitle;
    public string StatusTitle => Text.StatusTitle;
    public string HeroEyebrow => Text.HeroEyebrow;
    public string NowTunedForLabel => Text.NowTunedForLabel;
    public string HeroCardDetail => Text.HeroCardDetail;
    public string BehaviorTitle => Text.BehaviorTitle;
    public string IdleDelayCardTitle => Text.IdleDelayCardTitle;
    public string PatternCardTitle => Text.PatternCardTitle;
    public string TakeoverTitle => Text.TakeoverTitle;
    public string TakeoverValue => Text.TakeoverValue;
    public string HowItFeelsTitle => Text.HowItFeelsTitle;
    public string HowItFeelsBody => Text.HowItFeelsBody;
    public string ReplayTutorialLabel => Text.ReplayTutorialLabel;
    public string TrayReadyTitle => Text.TrayReadyTitle;
    public string TrayReadyBody => Text.TrayReadyBody;
    public string SettingsHeaderTitle => Text.SettingsHeaderTitle;
    public string SettingsSubtitle => Text.SettingsSubtitle;
    public string LookSectionTitle => Text.LookSectionTitle;
    public string BehaviorSectionTitle => Text.BehaviorSectionTitle;
    public string MovementSectionTitle => Text.MovementSectionTitle;
    public string ThemeLabel => Text.ThemeLabel;
    public string LanguageLabel => Text.LanguageLabel;
    public string ModeLabel => Text.ModeLabel;
    public string ColorVibeLabel => Text.ColorVibeLabel;
    public string LaunchInLabel => Text.LaunchInLabel;
    public string CloseBehaviorLabel => Text.CloseBehaviorLabel;
    public string IdleDelayLabel => Text.IdleDelayLabel;
    public string PatternLabel => Text.PatternLabel;
    public string SpeedLabel => Text.SpeedLabel;
    public string SizeLabel => Text.SizeLabel;
    public string ReadyCardValue => Text.ReadyCardValue;

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
            OnPropertyChanged(nameof(LocalizedSelectedPattern));
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

    public IReadOnlyList<AppLocalizationService.TutorialPageText> GetTutorialPages() =>
        Text.TutorialPages ?? Array.Empty<AppLocalizationService.TutorialPageText>();

    private AppLocalizationService.AppTextPack Text => _localizationService.GetPack(SelectedLanguageCode);

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

    private string LocalizeThemeMode(ThemeMode value) => value switch
    {
        ThemeMode.Light => SelectedLanguageCode switch
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
        ThemeMode.Dark => SelectedLanguageCode switch
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

    private string LocalizeThemeVibe(ThemeVibe value) => value switch
    {
        ThemeVibe.Tide => SelectedLanguageCode switch
        {
            "es" => "Marea",
            "pt" => "Maré",
            "fr" => "Marée",
            "de" => "Tide",
            "it" => "Marea",
            "nl" => "Getij",
            "sv" => "Tidvatten",
            "ja" => "タイド",
            "ko" => "타이드",
            "zh" => "潮汐",
            "ar" => "مد",
            "hi" => "टाइड",
            _ => "Tide"
        },
        ThemeVibe.Ember => SelectedLanguageCode switch
        {
            "es" => "Brasa",
            "pt" => "Brasa",
            "fr" => "Braise",
            "de" => "Glut",
            "it" => "Brace",
            "nl" => "Gloed",
            "sv" => "Glöd",
            "ja" => "エンバー",
            "ko" => "엠버",
            "zh" => "余烬",
            "ar" => "جمر",
            "hi" => "एम्बर",
            _ => "Ember"
        },
        ThemeVibe.Citrus => SelectedLanguageCode switch
        {
            "es" => "Cítrico",
            "pt" => "Cítrico",
            "fr" => "Agrume",
            "de" => "Zitrus",
            "it" => "Agrumi",
            "nl" => "Citrus",
            "sv" => "Citrus",
            "ja" => "シトラス",
            "ko" => "시트러스",
            "zh" => "柑橘",
            "ar" => "حمضيات",
            "hi" => "सिट्रस",
            _ => "Citrus"
        },
        ThemeVibe.Bloom => SelectedLanguageCode switch
        {
            "es" => "Bloom",
            "pt" => "Bloom",
            "fr" => "Bloom",
            "de" => "Bloom",
            "it" => "Bloom",
            "nl" => "Bloom",
            "sv" => "Bloom",
            "ja" => "ブルーム",
            "ko" => "블룸",
            "zh" => "绽放",
            "ar" => "ازدهار",
            "hi" => "ब्लूम",
            _ => "Bloom"
        },
        _ => value.ToString()
    };

    private void RefreshLocalizedProperties()
    {
        foreach (var property in new[]
        {
            nameof(StatusLabel), nameof(HeroLabel), nameof(BehaviorSummary), nameof(StatusDetail), nameof(ToggleButtonLabel),
            nameof(SettingsButtonLabel), nameof(LocalizedSelectedPattern), nameof(PatternSummary), nameof(BrandTitle), nameof(BrandSubtitle),
            nameof(StatusTitle), nameof(HeroEyebrow), nameof(NowTunedForLabel), nameof(HeroCardDetail), nameof(BehaviorTitle),
            nameof(IdleDelayCardTitle), nameof(PatternCardTitle), nameof(TakeoverTitle), nameof(TakeoverValue), nameof(HowItFeelsTitle),
            nameof(HowItFeelsBody), nameof(ReplayTutorialLabel), nameof(TrayReadyTitle), nameof(TrayReadyBody), nameof(SettingsHeaderTitle),
            nameof(SettingsSubtitle), nameof(LookSectionTitle), nameof(BehaviorSectionTitle), nameof(MovementSectionTitle),
            nameof(ThemeLabel), nameof(LanguageLabel), nameof(ModeLabel), nameof(ColorVibeLabel), nameof(LaunchInLabel),
            nameof(CloseBehaviorLabel), nameof(IdleDelayLabel), nameof(PatternLabel), nameof(SpeedLabel), nameof(SizeLabel),
            nameof(ReadyCardValue), nameof(ThemeModeItems), nameof(ThemeVibeItems), nameof(LaunchModeItems),
            nameof(CloseBehaviorItems), nameof(PatternItems), nameof(LanguageItems)
        })
        {
            OnPropertyChanged(property);
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
            "Figure 8" => "رقم 8",
            "Parallelogram" => "متوازي أضلاع",
            "Random" => "عشوائي",
            _ => pattern
        },
        "हिन्दी" => pattern switch
        {
            "Circle" => "सर्कल",
            "Square" => "स्क्वेयर",
            "Triangle" => "ट्रायंगल",
            "Figure 8" => "फिगर 8",
            "Parallelogram" => "पैरललोग्राम",
            "Random" => "रैंडम",
            _ => pattern
        },
        _ => pattern
    };
}
