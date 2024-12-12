// BasicInterop in WinUI3
// Example app from https://learn.microsoft.com/en-us/windows/apps/winui/winui3/desktop-winui3-app-with-basic-interop
// But when I wrote it, Web pages reference Nuget PInvoke.User32 that is officially deprecated.
// Replaced it by CsWin32, see https://www.nuget.org/packages/PInvoke.User32/0.7.124?_src=template
//
// 2024-12-12   PV

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BasicInterop
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            HWND hWnd = (HWND)WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            SetWindowDetails(hWnd, 800, 600);
            m_window.Activate();
        }

        private Window? m_window;

        // Converted original code in CsWin32 equivalent...
        private static void SetWindowDetails(HWND hwnd, int width, int height)
        {
            // Call GetDpiForWindow to get the dots per inch (dpi) value for the window (Win32 uses actual pixels while WinUI 3 uses effective pixels).
            // This dpi value is used to calculate the scale factor and apply it to the width and height specified for the window.
            var dpi = PInvoke.GetDpiForWindow(hwnd);
            float scalingFactor = (float)dpi / 96;
            width = (int)(width * scalingFactor);
            height = (int)(height * scalingFactor);

            _ = PInvoke.SetWindowPos(hwnd, HWND.HWND_TOP ,
                                            0, 0, width, height,
                                            Windows.Win32.UI.WindowsAndMessaging.SET_WINDOW_POS_FLAGS.SWP_NOMOVE);

            _ = PInvoke.SetWindowLong(hwnd,
                   Windows.Win32.UI.WindowsAndMessaging.WINDOW_LONG_PTR_INDEX.GWL_STYLE,
                   PInvoke.GetWindowLong(hwnd,
                      Windows.Win32.UI.WindowsAndMessaging.WINDOW_LONG_PTR_INDEX.GWL_STYLE) &
                      ~(int)WINDOW_STYLE.WS_MINIMIZEBOX &
                      ~(int)WINDOW_STYLE.WS_MAXIMIZEBOX);
        }
    }

}
