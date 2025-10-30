using System.Windows;
using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using Microsoft.Extensions.DependencyInjection;
using SignatureManagement.Wpf.Application.InitializeMain;
using SignatureManagement.Wpf.Main;

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

        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        MainWindow = serviceProvider.GetService<MainWindow>();
        MainWindow.Show();
    }
}

