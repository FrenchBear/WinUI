// 02_DrumPad
// Learning WinUI 3
// Tutorial from https://blogs.windows.com/windowsdeveloper/2022/01/28/build-your-first-winui-3-app-part-1/
//
// 2023-08-25   PV      Much shorter code than the original using Button's Tag

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using Microsoft.UI;
using Microsoft.UI.Windowing;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

//#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE1006 // Naming Styles

namespace WinUI02_DrumPad;

public sealed partial class MainWindow: Window
{
    readonly AppWindow m_appWindow;

    public MainWindow()
    {
        InitializeComponent();
        m_appWindow = GetAppWindowForCurrentWindow();
    }

    private AppWindow GetAppWindowForCurrentWindow()
    {
        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(myWndId);
    }

    private void SwtichPresenter_CompOverlay(object sender, RoutedEventArgs e) => m_appWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);

    private void SwtichPresenter_FullScreen(object sender, RoutedEventArgs e) => m_appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);

    private void SwtichPresenter_Default(object sender, RoutedEventArgs e) => m_appWindow.SetPresenter(AppWindowPresenterKind.Default);

    private void pad_clicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button b && b.Tag is string asset)
        {
            var installedPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            var soundFile = Path.Join(installedPath, "Assets", asset);
            var player = new System.Media.SoundPlayer(soundFile);
            player.Play();
        }
    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        var toggleSwitch = sender as ToggleSwitch;
        ((FrameworkElement)Content).RequestedTheme = toggleSwitch.IsOn ? ElementTheme.Dark : ElementTheme.Light;
    }
}
