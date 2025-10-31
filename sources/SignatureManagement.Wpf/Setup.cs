using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.CryptographyAccess;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Main;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Services;
using DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.SignatureManagement.Wpf;

internal static class Setup
{
    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
        // Infrastructure

        serviceCollection.AddAsyncMediator(typeof(InitializeMainRequest).Assembly);
        serviceCollection.AddSingleton<EventBus>();

        // GUI

        serviceCollection.AddSingleton<ThemeSelector>();

        serviceCollection.AddTransient<MainWindow>();
        
        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<KeysSelectorViewModel>();
        serviceCollection.AddTransient<KeyInfoViewModel>();
        serviceCollection.AddTransient<SigningPanelViewModel>();
        
        serviceCollection.AddTransient<SignMessageCommand>();
        serviceCollection.AddTransient<CreateKeyPairCommand>();
        serviceCollection.AddTransient<RefreshKeyPairsCommand>();
        serviceCollection.AddTransient<ToggleThemeCommand>();

        // External services

        serviceCollection.AddTransient<ISignatureKeyRepository, SignatureKeyRepository>();
        serviceCollection.AddSingleton<IApplicationState, ApplicationState>();
        serviceCollection.AddTransient<ICryptographyService, CryptographyService>();
    }
}