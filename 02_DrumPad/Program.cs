// Make DrumpPad a single instance app
// From https://blogs.windows.com/windowsdeveloper/2022/01/28/making-the-app-single-instanced-part-3/
//
// 2023-08-25   PV

using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;

namespace WinUI02_DrumPad;

class Program
{
    [STAThread]
    static async Task<int> Main(string[] args)
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();
        bool isRedirect = await DecideRedirection();
        if (!isRedirect)
        {
            Microsoft.UI.Xaml.Application.Start((p) =>
            {
                var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                _ = new App();
            });
        }

        return 0;
    }

    private static async Task<bool> DecideRedirection()
    {
        bool isRedirect = false;
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        ExtendedActivationKind kind = args.Kind;
        var keyInstance = AppInstance.FindOrRegisterForKey("randomKey");

        if (keyInstance.IsCurrent)
        {
            keyInstance.Activated += OnActivated;
        }
        else
        {
            isRedirect = true;
            await keyInstance.RedirectActivationToAsync(args);
        }
        return isRedirect;
    }

    private static void OnActivated(object sender, AppActivationArguments args)
    {
        ExtendedActivationKind _ = args.Kind;
    }
}
