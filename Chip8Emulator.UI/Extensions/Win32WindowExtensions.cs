// Taken from http://stackoverflow.com/a/315436/4153185 to enable usage of FolderBrowserDialog from WPF
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace Chip8Emulator.UI.Extensions
{
    public static class Win32WindowExtensions
    {
        public static IWin32Window GetIWin32Window(this Visual visual)
        {
            var source = PresentationSource.FromVisual(visual) as HwndSource;
            IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        private class OldWindow : IWin32Window
        {
            private readonly IntPtr _handle;
            public OldWindow(IntPtr handle)
            {
                _handle = handle;
            }

            IntPtr IWin32Window.Handle
            {
                get { return _handle; }
            }
        }
    }
}
