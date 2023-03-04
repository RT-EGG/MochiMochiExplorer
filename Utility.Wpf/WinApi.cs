using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Utility.Wpf
{
    public static class WinApi
    {
        public static void SetSystemMenuVisibility(Window inWindow, bool inVisible)
        {
            var handle = new WindowInteropHelper(inWindow).Handle;

            var style = GetWindowLong(handle, GWL_STYLE);
            if (inVisible)
                style |= WS_SYSMENU;
            else
                style &= ~WS_SYSMENU;

            SetWindowLong(handle, GWL_STYLE, style);
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        const int GWL_STYLE = -16;
        const int WS_SYSMENU = 0x80000;
    }
}
