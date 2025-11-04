using System.IO;
using AsyncMediator.Extensions.DependencyInjection;
using DustInTheWind.CryptoKeyManagement.Adapter.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptographyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Ports.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.SignatureFormatting.DependencyInjection;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializeApp;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Watchers;
using DustInTheWind.CryptoKeyManagement.Wpf.Main;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Dialogs;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeysSelector;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Sidebar;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPage;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPanel;
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
        
        serviceCollection.AddTransient<SidebarViewModel>();

        serviceCollection.AddTransient<SigningPageViewModel>();
        serviceCollection.AddTransient<KeysSelectorViewModel>();
        serviceCollection.AddTransient<KeyInfoViewModel>();
        serviceCollection.AddTransient<SigningPanelViewModel>();

        serviceCollection.AddTransient<PluginsPageViewModel>();

        serviceCollection.AddTransient<SignMessageCommand>();
        serviceCollection.AddTransient<CreateKeyPairCommand>();
        serviceCollection.AddTransient<RefreshKeyPairsCommand>();
        serviceCollection.AddTransient<DeleteKeyPairCommand>();
        serviceCollection.AddTransient<ToggleThemeCommand>();

        // Presentation services
        serviceCollection.AddSignatureFormattingPlugins(options =>
        {
            _ = options.AddFromCurrentApplicationDomain();

            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string pluginsDirectory = Path.Combine(userProfile, "Crypto Key Management", "Plugins");
            _ = options.AddFromDirectory(pluginsDirectory, includeSubdirectories: true);
        });

        // Dialog services
        serviceCollection.AddTransient<IDialogService, DialogService>();

        // Miscellanneous

        serviceCollection.AddSingleton<ApplicationStateWatcher>();

        // External services

        serviceCollection.AddTransient<ICryptoKeyRepository, CryptoKeyRepository>();
        serviceCollection.AddSingleton<IApplicationState, ApplicationState>();
        serviceCollection.AddTransient<ICryptographyService, CryptographyService>();
    }
}