using System.Windows;
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
}

