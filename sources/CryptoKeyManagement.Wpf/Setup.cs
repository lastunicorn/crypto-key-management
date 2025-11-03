using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.CryptoKeyManagement.Adapter.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptographyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Ports.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.SignatureFormatting;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializeApp;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Watchers;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Dialogs;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeysSelector;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Sidebar;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPanel;
using DustInTheWind.CryptoKeyManagement.Wpf.Main;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.CryptoKeyManagement.Wpf;

internal static class Setup
{
    public static void ConfigureServices(ServiceCollection serviceCollection)
    {
        // Infrastructure

        serviceCollection.AddAsyncMediator(typeof(InitializeAppRequest).Assembly);
        serviceCollection.AddSingleton<IEventBus, EventBus>();

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
        serviceCollection.AddSingleton<SignatureFormatterPool>();

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