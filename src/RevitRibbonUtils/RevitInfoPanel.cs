using System;
using System.Runtime.InteropServices;
using Autodesk.Windows;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils
{
    public class RevitInfoPanel
    {
        public void ShowMessage(string message)
        {
            var mainWindow = ComponentManager.ApplicationWindow;

            var statusBar = FindWindowEx(mainWindow, IntPtr.Zero, "msctls_statusbar32", "");

            if (statusBar != IntPtr.Zero)
                SetWindowText(statusBar, message);
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
    }
}