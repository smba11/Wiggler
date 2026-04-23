using System.Runtime.InteropServices;

namespace WigglerBySmba.Services;

public sealed class MouseHookService : IDisposable
{
    private readonly NativeMethods.LowLevelMouseProc _hookCallback;
    private nint _hookHandle;

    public event EventHandler? UserMouseActivity;

    public MouseHookService()
    {
        _hookCallback = HookCallback;
        _hookHandle = NativeMethods.SetWindowsHookEx(NativeMethods.WhMouseLl, _hookCallback, nint.Zero, 0);
    }

    private nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode < 0)
        {
            return NativeMethods.CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }

        var message = unchecked((int)wParam);
        if (message is NativeMethods.WmMouseMove or NativeMethods.WmLButtonDown or NativeMethods.WmRButtonDown or NativeMethods.WmMouseWheel or NativeMethods.WmMButtonDown or NativeMethods.WmMouseHWheel)
        {
            var mouseInfo = Marshal.PtrToStructure<NativeMethods.MsllHookStruct>(lParam);
            if (mouseInfo.DwExtraInfo != NativeMethods.WigglerExtraInfo)
            {
                UserMouseActivity?.Invoke(this, EventArgs.Empty);
            }
        }

        return NativeMethods.CallNextHookEx(_hookHandle, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        if (_hookHandle == nint.Zero)
        {
            return;
        }

        NativeMethods.UnhookWindowsHookEx(_hookHandle);
        _hookHandle = nint.Zero;
    }
}
