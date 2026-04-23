namespace WigglerBySmba;

public partial class App : System.Windows.Application
{
    protected override void OnExit(System.Windows.ExitEventArgs e)
    {
        if (Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.PrepareForExit();
        }

        base.OnExit(e);
    }
}
