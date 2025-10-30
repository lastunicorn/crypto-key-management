using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Main;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeysPanel;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;
using DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.SignatureManagement.Wpf;

internal static class Setup
{
    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddAsyncMediator(typeof(InitializeMainRequest).Assembly);
        serviceCollection.AddSingleton<EventBus>();

        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<KeysPanelViewModel>();
        serviceCollection.AddTransient<KeyInfoViewModel>();
        serviceCollection.AddTransient<SigningPanelViewModel>();
        serviceCollection.AddTransient<ISignatureKeyRepository, SignatureKeyRepository>();

        serviceCollection.AddSingleton<IApplicationState, ApplicationState>();
    }
}