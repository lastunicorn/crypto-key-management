using System.Windows;
using System.Windows.Threading;
using DustInTheWind.SignatureManagement.Wpf.Application.Watchers;
using DustInTheWind.SignatureManagement.Wpf.Main;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.SignatureManagement.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        SetupExceptionHandling();

        ServiceCollection serviceCollection = new();
        Setup.ConfigureServices(serviceCollection);
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        Initialize(serviceProvider);

        MainWindow = serviceProvider.GetService<MainWindow>();
        MainWindow.Show();
    }

    private void Initialize(IServiceProvider serviceProvider)
    {
        ApplicationStateWatcher applicationStateWatcher = serviceProvider.GetService<ApplicationStateWatcher>();
        applicationStateWatcher?.Start();
    }

    private void SetupExceptionHandling()
    {
        // Handle exceptions on the UI thread
        DispatcherUnhandledException += OnDispatcherUnhandledException;

        // Handle exceptions on non-UI threads
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

        // Handle task exceptions
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ShowExceptionMessage(e.Exception, "UI Thread Exception");
        e.Handled = true;
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
            ShowExceptionMessage(exception, "Application Exception");
    }

    private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        ShowExceptionMessage(e.Exception, "Task Exception");
        e.SetObserved();
    }

    private void ShowExceptionMessage(Exception exception, string title)
    {
        try
        {
            string message = $"An unexpected error occurred:\n\n" +
               $"Type: {exception.GetType().Name}\n" +
             $"Message: {exception.Message}\n\n" +
             $"Stack Trace:\n{exception.StackTrace}";

            // Use Dispatcher.Invoke to ensure the message box is shown on the UI thread
            Dispatcher.Invoke(() =>
             {
                 MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
             });
        }
        catch
        {
            // If we can't show the message box for any reason, fail silently
            // to prevent recursive exceptions
        }
    }
}

