using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.SignatureManagement.Adapter.WpfUserAccess;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.CryptographyAccess;
using DustInTheWind.SignatureManagement.Ports.SettingsAccess;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Ports.WpfUserAccess;
using DustInTheWind.SignatureManagement.SignatureFormatting;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeApp;
using DustInTheWind.SignatureManagement.Wpf.Application.Watchers;
using DustInTheWind.SignatureManagement.Wpf.Main;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Dialogs;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Sidebar;
using DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.SignatureManagement.Wpf;

internal static class Setup
{
    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
        // Infrastructure

        serviceCollection.AddAsyncMediator(typeof(InitializeAppRequest).Assembly);
        serviceCollection.AddSingleton<EventBus>();

        // GUI

        serviceCollection.AddSingleton<ISettingsService, SettingsService>();
        serviceCollection.AddSingleton<IThemeSelector, ThemeSelector>();

        serviceCollection.AddTransient<MainWindow>();

        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<KeysSelectorViewModel>();
        serviceCollection.AddTransient<KeyInfoViewModel>();
        serviceCollection.AddTransient<SigningPanelViewModel>();
        serviceCollection.AddTransient<SidebarViewModel>();

        serviceCollection.AddTransient<SignMessageCommand>();
        serviceCollection.AddTransient<CreateKeyPairCommand>();
        serviceCollection.AddTransient<RefreshKeyPairsCommand>();
        serviceCollection.AddTransient<DeleteKeyPairCommand>();
        serviceCollection.AddTransient<ToggleThemeCommand>();

        // Presentation services
        serviceCollection.AddTransient<ISignatureFormatter, Base64SignatureFormatter>();

        // Dialog services
        serviceCollection.AddTransient<IDialogService, DialogService>();

        // Miscellanneous

        serviceCollection.AddSingleton<ApplicationStateWatcher>();

        // External services

        serviceCollection.AddTransient<ISignatureKeyRepository, SignatureKeyRepository>();
        serviceCollection.AddSingleton<IApplicationState, ApplicationState>();
        serviceCollection.AddTransient<ICryptographyService, CryptographyService>();
    }
}