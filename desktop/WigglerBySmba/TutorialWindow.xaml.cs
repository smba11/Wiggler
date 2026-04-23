using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WigglerBySmba;

public partial class TutorialWindow : Window, INotifyPropertyChanged
{
    private readonly List<TutorialPage> _pages =
    [
        new("A calm mouse wiggler with boundaries.",
            "Turn WIGGLER on, step away, and it waits until you have actually been idle before it takes over.",
            "It stays friendly by arming first, then moving only after the idle timer expires."),
        new("Turning it on is the whole flow.",
            "Use the main power button to arm it. While armed, WIGGLER watches for real user input and stays out of your way.",
            "The default idle delay is 12 seconds, and you can change that anytime in Settings."),
        new("Patterns stay visible and controlled.",
            "Choose Circle, Square, Triangle, Figure 8, Parallelogram, or Random.",
            "Patterns stay on-screen, redirect near edges, and travel smoothly over time instead of jumping around."),
        new("You always win instantly.",
            "The moment you move the mouse yourself, WIGGLER stops immediately and returns to the armed state.",
            "Its own movement is tagged and ignored, so it never mistakes itself for your input."),
        new("Tray support keeps it tucked away.",
            "Run it in the background, reopen it from the tray, and replay this tutorial any time from Settings.",
            "You can choose whether the app launches in a window or the tray, and whether closing hides it or exits.")
    ];

    private int _pageIndex;
    private string _titleText = string.Empty;
    private string _bodyText = string.Empty;
    private string _detailText = string.Empty;

    public TutorialWindow()
    {
        InitializeComponent();
        DataContext = this;
        RefreshPage();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string TitleText
    {
        get => _titleText;
        private set
        {
            _titleText = value;
            OnPropertyChanged();
        }
    }

    public string BodyText
    {
        get => _bodyText;
        private set
        {
            _bodyText = value;
            OnPropertyChanged();
        }
    }

    public string DetailText
    {
        get => _detailText;
        private set
        {
            _detailText = value;
            OnPropertyChanged();
        }
    }

    public string ProgressText => $"{_pageIndex + 1} of {_pages.Count}";

    public string NextButtonText => _pageIndex == _pages.Count - 1 ? "Finish" : "Next";

    private void RefreshPage()
    {
        var page = _pages[_pageIndex];
        TitleText = page.Title;
        BodyText = page.Body;
        DetailText = page.Detail;
        OnPropertyChanged(nameof(ProgressText));
        OnPropertyChanged(nameof(NextButtonText));
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        if (_pageIndex == 0)
        {
            return;
        }

        _pageIndex--;
        RefreshPage();
    }

    private void Next_Click(object sender, RoutedEventArgs e)
    {
        if (_pageIndex == _pages.Count - 1)
        {
            DialogResult = true;
            Close();
            return;
        }

        _pageIndex++;
        RefreshPage();
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private sealed record TutorialPage(string Title, string Body, string Detail);
}
