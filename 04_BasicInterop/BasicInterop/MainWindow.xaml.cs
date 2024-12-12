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
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace BasicInterop
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private async void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MyButton.Content = "Clicked";

            var description = new System.Text.StringBuilder();
            var process = System.Diagnostics.Process.GetCurrentProcess();
            foreach (System.Diagnostics.ProcessModule module in process.Modules)
            {
                description.AppendLine(module.FileName);
            }

            cdTextBlock.Text = description.ToString();
            await contentDialog.ShowAsync();
        }
    }
}
