using System.Drawing;
using System.Windows.Forms;
using WigglerBySmba.Models;

namespace WigglerBySmba.Services;

public sealed class TrayIconService : IDisposable
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ToolStripMenuItem _statusItem;
    private bool _hasShownTrayTip;

    public event EventHandler? OpenRequested;
    public event EventHandler? SettingsRequested;
    public event EventHandler? ExitRequested;

    public TrayIconService()
    {
        _statusItem = new ToolStripMenuItem("Status: Off")
        {
            Enabled = false
        };

        var openItem = new ToolStripMenuItem("Open window");
        openItem.Click += (_, _) => OpenRequested?.Invoke(this, EventArgs.Empty);

        var settingsItem = new ToolStripMenuItem("Settings");
        settingsItem.Click += (_, _) => SettingsRequested?.Invoke(this, EventArgs.Empty);

        var exitItem = new ToolStripMenuItem("Exit");
        exitItem.Click += (_, _) => ExitRequested?.Invoke(this, EventArgs.Empty);

        _notifyIcon = new NotifyIcon
        {
            Text = "WIGGLER by SMBA",
            Icon = SystemIcons.Application,
            ContextMenuStrip = new ContextMenuStrip()
        };

        _notifyIcon.ContextMenuStrip.Items.AddRange([_statusItem, openItem, settingsItem, exitItem]);
        _notifyIcon.DoubleClick += (_, _) => OpenRequested?.Invoke(this, EventArgs.Empty);
    }

    public void Initialize()
    {
        _notifyIcon.Visible = true;
    }

    public void EnsureVisible()
    {
        _notifyIcon.Visible = true;
    }

    public void ShowRunningInTrayTip()
    {
        if (_hasShownTrayTip)
        {
            return;
        }

        _notifyIcon.BalloonTipTitle = "WIGGLER is still running";
        _notifyIcon.BalloonTipText = "Open it from the tray at any time.";
        _notifyIcon.ShowBalloonTip(2500);
        _hasShownTrayTip = true;
    }

    public void UpdateStatus(WigglerStatus status)
    {
        _statusItem.Text = $"Status: {status}";
    }

    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
    }
}
