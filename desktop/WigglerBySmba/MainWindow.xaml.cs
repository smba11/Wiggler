using System.ComponentModel;
using System.Windows;
using WigglerBySmba.Models;
using WigglerBySmba.Services;
using WigglerBySmba.ViewModels;

namespace WigglerBySmba;

public partial class MainWindow : Window
{
    private const double CompactWidth = 1060;
    private const double ExpandedWidth = 1480;
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
        _viewModel.StatusChanged += (_, status) => _trayIconService.UpdateStatus(status);
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
            EasingFunction = new System.Windows.Media.Animation.CubicEase
            {
                EasingMode = System.Windows.Media.Animation.EasingMode.EaseOut
            }
        });
    }
}
