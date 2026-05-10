using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using WigglerBySmba.Models;
using WigglerBySmba.Services;
using WigglerBySmba.ViewModels;

namespace WigglerBySmba;

public partial class MainWindow : Window
{
    private const double CompactWidth = 1040;
    private const double ExpandedWidth = 1460;
    private const double CompactMinWidth = 900;
    private const double ExpandedMinWidth = 1260;
    private readonly SettingsService _settingsService;
    private readonly MouseHookService _mouseHookService;
    private readonly MouseMovementService _mouseMovementService;
    private readonly TrayIconService _trayIconService;
    private readonly ThemeService _themeService;
    private readonly MainViewModel _viewModel;
    private bool _isExiting;
    private bool _isDisposed;

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
        _trayIconService.UpdateStatus(_viewModel.Status);

        Loaded += OnLoaded;
        StateChanged += OnStateChanged;
        Closing += OnClosing;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _viewModel.Initialize();
        ApplySettingsLayout(_viewModel.IsSettingsOpen, false);
        ApplyToggleVisualState(_viewModel.IsPoweredOn);
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.IsSettingsOpen))
        {
            ApplySettingsLayout(_viewModel.IsSettingsOpen, true);
        }
    }

    private void OnStateChanged(object? sender, EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            _trayIconService.EnsureVisible();
            if (_viewModel.SelectedLaunchMode == LaunchMode.Tray || _viewModel.SelectedCloseBehavior == CloseBehavior.MinimizeToTray)
            {
                HideToTray();
            }
        }
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        if (_isExiting || _viewModel.SelectedCloseBehavior == CloseBehavior.Exit)
        {
            PrepareForExit();
            return;
        }

        e.Cancel = true;
        HideToTray();
    }

    private void ShowTutorialDialog()
    {
        var wasHidden = !IsVisible;
        if (wasHidden)
        {
            RevealWindow();
        }

        var tutorialWindow = new TutorialWindow(_viewModel.SelectedLanguageCode)
        {
            Owner = this
        };

        tutorialWindow.ShowDialog();
        _viewModel.CompleteOnboarding();
    }

    private void HideToTray()
    {
        Hide();
        ShowInTaskbar = false;
        _trayIconService.EnsureVisible();
        _trayIconService.ShowRunningInTrayTip();
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
    }

    private void ApplySettingsLayout(bool isOpen, bool animateWidth)
    {
        SettingsSpacerColumn.Width = isOpen ? new GridLength(28) : new GridLength(0);
        SettingsColumn.Width = isOpen ? new GridLength(1.02, GridUnitType.Star) : new GridLength(0);
        SettingsShell.Visibility = isOpen ? Visibility.Visible : Visibility.Collapsed;

        MinWidth = isOpen ? ExpandedMinWidth : CompactMinWidth;
        var targetWidth = isOpen ? ExpandedWidth : CompactWidth;

        if (!animateWidth)
        {
            Width = targetWidth;
            return;
        }

        BeginAnimation(WidthProperty, new System.Windows.Media.Animation.DoubleAnimation
        {
            To = targetWidth,
            Duration = TimeSpan.FromMilliseconds(220),
            EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            }
        });
    }

    private void AnimateStatusChange(WigglerStatus status)
    {
        var poweredOn = status is WigglerStatus.Armed or WigglerStatus.Running;
        ApplyToggleVisualState(poweredOn);

        var knobTarget = poweredOn ? -174d : 0d;
        var glowTarget = poweredOn ? 0.88d : 0.28d;
        var glowScale = poweredOn ? 1.08d : 0.92d;

        ToggleKnobTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, new DoubleAnimation
        {
            To = knobTarget,
            Duration = TimeSpan.FromMilliseconds(320),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        });

        ToggleGlow.BeginAnimation(OpacityProperty, new DoubleAnimation
        {
            To = glowTarget,
            Duration = TimeSpan.FromMilliseconds(260),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        });

        ToggleGlowScale.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, new DoubleAnimation
        {
            To = glowScale,
            Duration = TimeSpan.FromMilliseconds(260),
            AutoReverse = poweredOn,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        });

        ToggleGlowScale.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, new DoubleAnimation
        {
            To = glowScale,
            Duration = TimeSpan.FromMilliseconds(260),
            AutoReverse = poweredOn,
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        });
    }

    private void ApplyToggleVisualState(bool poweredOn)
    {
        ToggleKnobTransform.X = poweredOn ? -174d : 0d;
        ToggleGlow.Opacity = poweredOn ? 0.72d : 0.28d;
        ToggleGlowScale.ScaleX = 1d;
        ToggleGlowScale.ScaleY = 1d;
    }
}
