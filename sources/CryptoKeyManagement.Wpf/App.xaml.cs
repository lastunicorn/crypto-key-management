using System.Windows;
using System.Windows.Threading;
using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Wpf;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializeApp;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Dialogs;
using DustInTheWind.CryptoKeyManagement.Wpf.Main;
using Microsoft.Extensions.DependencyInjection;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;

namespace DustInTheWind.CryptoKeyManagement.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IServiceProvider serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        SetupExceptionHandling();

        ServiceCollection serviceCollection = new();
        Setup.ConfigureServices(serviceCollection);
        serviceProvider = serviceCollection.BuildServiceProvider();

        _ = Initialize(serviceProvider);
    }

    private async Task Initialize(IServiceProvider serviceProvider)
    {
        IMediator mediator = serviceProvider.GetService<IMediator>();

        InitializeAppRequest command = new();
        await mediator.Send(command);

        MainWindow = serviceProvider.GetService<MainWindow>();
        MainWindow.DataContext = serviceProvider.GetService<MainViewModel>();
        MainWindow.Show();
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

            // Use Dispatcher.Invoke to ensure the dialog is shown on the UI thread
            Dispatcher.Invoke(() =>
            {
                try
                {
                    // Try to use DialogService if available
                    var dialogService = serviceProvider?.GetService<IDialogService>();
                    if (dialogService != null)
                    {
                        dialogService.ShowErrorDialog(title, message);
                    }
                    else
                    {
                        // Fallback to MessageBox if DialogService is not available
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch
                {
                    // If DialogService fails, fallback to MessageBox
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
        catch
        {
            // If we can't show the message at all for any reason, fail silently
            // to prevent recursive exceptions
        }
    }
}

