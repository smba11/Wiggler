using System.ComponentModel;
using System.Windows;
using WigglerBySmba.Models;
using WigglerBySmba.Services;
using WigglerBySmba.ViewModels;

namespace WigglerBySmba;

public partial class MainWindow : Window
{
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

        if (_viewModel.ShouldLaunchInTray)
        {
            HideToTray();
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

        var tutorialWindow = new TutorialWindow
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
}
