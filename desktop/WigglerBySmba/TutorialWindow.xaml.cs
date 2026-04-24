using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WigglerBySmba.Services;

namespace WigglerBySmba;

public partial class TutorialWindow : Window, INotifyPropertyChanged
{
    private readonly List<AppLocalizationService.TutorialPageText> _pages;
    private readonly string _languageCode;
    private int _pageIndex;
    private string _titleText = string.Empty;
    private string _bodyText = string.Empty;
    private string _detailText = string.Empty;

    public TutorialWindow(string languageCode)
    {
        _languageCode = languageCode;
        var localization = new AppLocalizationService();
        _pages = localization.GetPack(languageCode).TutorialPages?.ToList()
            ?? localization.GetPack("en").TutorialPages!.ToList();

        InitializeComponent();
        DataContext = this;
        RefreshPage();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string WindowTitle => _languageCode switch
    {
        "es" => "Bienvenido a WIGGLER by SMBA",
        "pt" => "Bem-vindo ao WIGGLER by SMBA",
        "fr" => "Bienvenue sur WIGGLER by SMBA",
        "de" => "Willkommen bei WIGGLER by SMBA",
        "it" => "Benvenuto in WIGGLER by SMBA",
        "nl" => "Welkom bij WIGGLER by SMBA",
        "sv" => "Välkommen till WIGGLER by SMBA",
        "ja" => "WIGGLER by SMBA へようこそ",
        "ko" => "WIGGLER by SMBA에 오신 것을 환영합니다",
        "zh" => "欢迎使用 WIGGLER by SMBA",
        "ar" => "مرحباً بك في WIGGLER by SMBA",
        "hi" => "WIGGLER by SMBA में आपका स्वागत है",
        _ => "Welcome to WIGGLER by SMBA"
    };

    public string BrandText => "WIGGLER by SMBA";

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

    public string ProgressText => $"{_pageIndex + 1} / {_pages.Count}";

    public string BackButtonText => _languageCode switch
    {
        "es" => "Atrás",
        "pt" => "Voltar",
        "fr" => "Retour",
        "de" => "Zurück",
        "it" => "Indietro",
        "nl" => "Terug",
        "sv" => "Tillbaka",
        "ja" => "戻る",
        "ko" => "뒤로",
        "zh" => "返回",
        "ar" => "رجوع",
        "hi" => "पीछे",
        _ => "Back"
    };

    public string NextButtonText => _pageIndex == _pages.Count - 1
        ? _languageCode switch
        {
            "es" => "Finalizar",
            "pt" => "Concluir",
            "fr" => "Terminer",
            "de" => "Fertig",
            "it" => "Fine",
            "nl" => "Afronden",
            "sv" => "Slutför",
            "ja" => "完了",
            "ko" => "완료",
            "zh" => "完成",
            "ar" => "إنهاء",
            "hi" => "समाप्त",
            _ => "Finish"
        }
        : _languageCode switch
        {
            "es" => "Siguiente",
            "pt" => "Próximo",
            "fr" => "Suivant",
            "de" => "Weiter",
            "it" => "Avanti",
            "nl" => "Volgende",
            "sv" => "Nästa",
            "ja" => "次へ",
            "ko" => "다음",
            "zh" => "继续",
            "ar" => "التالي",
            "hi" => "आगे",
            _ => "Next"
        };

    private void RefreshPage()
    {
        var page = _pages[_pageIndex];
        TitleText = page.Title;
        BodyText = page.Body;
        DetailText = page.Detail;
        OnPropertyChanged(nameof(WindowTitle));
        OnPropertyChanged(nameof(ProgressText));
        OnPropertyChanged(nameof(BackButtonText));
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
}
