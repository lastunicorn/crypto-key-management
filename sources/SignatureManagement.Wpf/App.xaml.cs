using System.Windows;
using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;
using Microsoft.Extensions.DependencyInjection;
using DustInTheWind.SignatureManagement.Wpf.Main;
using DustInTheWind.SignatureManagement.Ports.StateAccess;

namespace SignatureManagement.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ServiceCollection serviceCollection = new();

        serviceCollection.AddAsyncMediator(typeof(InitializeMainRequest).Assembly);

        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<ISignatureKeyRepository, SignatureKeyRepository>();

        // Register application state service as singleton to maintain state across the application
        serviceCollection.AddSingleton<IApplicationState, ApplicationState>();

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        MainWindow = serviceProvider.GetService<MainWindow>();
        MainWindow.Show();
    }
}

