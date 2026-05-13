using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WigglerBySmba.Models;
using WigglerBySmba.Services;
using WigglerBySmba.ViewModels;

namespace WigglerBySmba;

public partial class MainWindow : Window
{
    private const double ToggleKnobOnX = 146d;
    private const double ToggleKnobOffX = 0d;
    private const double ToggleGlowOnOpacity = 0.08d;
    private const double ToggleGlowOffOpacity = 0.0d;
    private const double SettingsOpenWidth = 392d;
    private const double SettingsClosedWidth = 0d;
    private const double SettingsOpenOffset = 0d;
    private const double SettingsClosedOffset = 28d;

    private readonly SettingsService _settingsService;
    private readonly MouseHookService _mouseHookService;
    private readonly MouseMovementService _mouseMovementService;
    private readonly TrayIconService _trayIconService;
    private readonly ThemeService _themeService;
    private readonly MainViewModel _viewModel;
    private readonly System.Windows.Threading.DispatcherTimer _moveStopTimer;
    private bool _isExiting;
    private bool _isDisposed;
    private bool _isTutorialOpen;
    private bool _isMoveOptimized;

    public MainWindow()
    {
        InitializeComponent();

        _settingsService = new SettingsService();
        var settings = _settingsService.Load();

        _mouseHookService = new MouseHookService();
        _mouseMovementService = new MouseMovementService();
        _trayIconService = new TrayIconService();
        _themeService = new ThemeService();
        _viewModel = new MainViewModel(settings, _settingsService, _mouseHookService, _mouseMovementService);
        _moveStopTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(140)
        };
        _moveStopTimer.Tick += OnMoveStopTimerTick;

        DataContext = _viewModel;

        _viewModel.RequestTutorial += (_, _) => ShowTutorialDialog();
        _viewModel.StatusChanged += (_, status) =>
        {
            _trayIconService.UpdateStatus(status);
            AnimateStatusChange(status);
        };
        _viewModel.ThemeChanged += (_, _) => _themeService.ApplyTheme(_viewModel.SelectedThemeMode, _viewModel.SelectedThemeVibe);
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;

        _trayIconService.OpenRequested += (_, _) => RevealWindow();
        _trayIconService.SettingsRequested += (_, _) =>
        {
            RevealWindow();
            _viewModel.IsSettingsOpen = true;
        };
        _trayIconService.ExitRequested += (_, _) =>
        {
            _isExiting = true;
            Close();
        };

        _trayIconService.Initialize();
        _trayIconService.SetVisible(_viewModel.ShowTrayIcon);
        _trayIconService.UpdateStatus(_viewModel.Status);

        Loaded += OnLoaded;
        StateChanged += OnStateChanged;
        Closing += OnClosing;
        LocationChanged += OnLocationChanged;
        SizeChanged += OnSizeChanged;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _viewModel.Initialize();
        ApplySettingsLayout(_viewModel.IsSettingsOpen, animate: false);
        ApplyToggleVisualState(_viewModel.IsPoweredOn);
        UpdateWindowChromeState();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.IsSettingsOpen))
        {
            ApplySettingsLayout(_viewModel.IsSettingsOpen);
        }
        else if (e.PropertyName == nameof(MainViewModel.ShowTrayIcon))
        {
            _trayIconService.SetVisible(_viewModel.ShowTrayIcon);
        }
    }

    private void OnStateChanged(object? sender, EventArgs e)
    {
        UpdateWindowChromeState();

        if (WindowState == WindowState.Minimized)
        {
            if (_viewModel.ShowTrayIcon)
            {
                _trayIconService.EnsureVisible();
            }

            if (_viewModel.SelectedLaunchMode == LaunchMode.Tray ||
                (_viewModel.SelectedCloseBehavior == CloseBehavior.MinimizeToTray && _viewModel.KeepRunningInBackground))
            {
                HideToTray();
            }
        }
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        if (_isExiting || !_viewModel.KeepRunningInBackground || _viewModel.SelectedCloseBehavior == CloseBehavior.Exit)
        {
            PrepareForExit();
            return;
        }

        e.Cancel = true;
        HideToTray();
    }

    private void ShowTutorialDialog()
    {
        if (_isTutorialOpen)
        {
            return;
        }

        _isTutorialOpen = true;
        var wasHidden = !IsVisible;
        try
        {
            if (wasHidden)
            {
                RevealWindow();
            }

            var tutorialWindow = new TutorialWindow(_viewModel.SelectedLanguageCode);
            if (IsVisible)
            {
                tutorialWindow.Owner = this;
            }

            tutorialWindow.ShowDialog();
            _viewModel.CompleteOnboarding();
        }
        finally
        {
            _isTutorialOpen = false;
        }
    }

    private void HideToTray()
    {
        Hide();
        ShowInTaskbar = false;
        if (_viewModel.ShowTrayIcon)
        {
            _trayIconService.EnsureVisible();
            _trayIconService.ShowRunningInTrayTip();
        }
    }

    private void RevealWindow()
    {
        ShowInTaskbar = true;
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

    public void PrepareForExit()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _viewModel.Dispose();
        _trayIconService.Dispose();
        _moveStopTimer.Stop();
        _moveStopTimer.Tick -= OnMoveStopTimerTick;
    }

    private void ApplySettingsLayout(bool isOpen, bool animate = true)
    {
        SettingsShell.IsHitTestVisible = isOpen;

        var targetWidth = isOpen ? SettingsOpenWidth : SettingsClosedWidth;
        var targetOpacity = isOpen ? 1d : 0d;
        var targetOffset = isOpen ? SettingsOpenOffset : SettingsClosedOffset;

        if (!animate)
        {
            SettingsShell.Width = targetWidth;
            SettingsShell.Opacity = targetOpacity;
            SettingsTranslateTransform.X = targetOffset;
            return;
        }

        var ease = new CubicEase { EasingMode = EasingMode.EaseOut };
        SettingsShell.BeginAnimation(WidthProperty, new DoubleAnimation
        {
            To = targetWidth,
            Duration = TimeSpan.FromMilliseconds(240),
            EasingFunction = ease
        });

        SettingsShell.BeginAnimation(OpacityProperty, new DoubleAnimation
        {
            To = targetOpacity,
            Duration = TimeSpan.FromMilliseconds(180),
            EasingFunction = ease
        });

        SettingsTranslateTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, new DoubleAnimation
        {
            To = targetOffset,
            Duration = TimeSpan.FromMilliseconds(240),
            EasingFunction = ease
        });
    }

    private void AnimateStatusChange(WigglerStatus status)
    {
        var poweredOn = status is WigglerStatus.Armed or WigglerStatus.Running;
        ApplyToggleVisualState(poweredOn);

        ToggleKnobTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, new DoubleAnimation
        {
            To = poweredOn ? ToggleKnobOnX : ToggleKnobOffX,
            Duration = TimeSpan.FromMilliseconds(260),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        });

        ToggleGlow.BeginAnimation(OpacityProperty, new DoubleAnimation
        {
            To = poweredOn ? ToggleGlowOnOpacity : ToggleGlowOffOpacity,
            Duration = TimeSpan.FromMilliseconds(220),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        });
    }

    private void ApplyToggleVisualState(bool poweredOn)
    {
        ToggleKnobTransform.X = poweredOn ? ToggleKnobOnX : ToggleKnobOffX;
        ToggleGlow.Opacity = poweredOn ? ToggleGlowOnOpacity : ToggleGlowOffOpacity;
    }

    private void OnLocationChanged(object? sender, EventArgs e)
    {
        OptimizeForMove();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (!e.WidthChanged && !e.HeightChanged)
        {
            return;
        }

        OptimizeForMove();
    }

    private void OptimizeForMove()
    {
        if (!_isMoveOptimized)
        {
            _isMoveOptimized = true;
            MainShell.Effect = null;
            SettingsShell.Effect = null;
            MainShell.CacheMode = new BitmapCache();
            SettingsShell.CacheMode = new BitmapCache();
        }

        _moveStopTimer.Stop();
        _moveStopTimer.Start();
    }

    private void OnMoveStopTimerTick(object? sender, EventArgs e)
    {
        _moveStopTimer.Stop();
        if (!_isMoveOptimized)
        {
            return;
        }

        _isMoveOptimized = false;
        MainShell.ClearValue(EffectProperty);
        SettingsShell.ClearValue(EffectProperty);
        MainShell.CacheMode = null;
        SettingsShell.CacheMode = null;
    }

    private void UpdateWindowChromeState()
    {
        var isMaximized = WindowState == WindowState.Maximized;
        ChromeRoot.Margin = isMaximized ? new Thickness(6) : new Thickness(14);
        MainShell.CornerRadius = isMaximized ? new CornerRadius(20) : new CornerRadius(28);
        MaximizeButton.Content = isMaximized ? "\uE923" : "\uE922";
    }

    private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeRestoreWindow_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void CloseWindow_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
