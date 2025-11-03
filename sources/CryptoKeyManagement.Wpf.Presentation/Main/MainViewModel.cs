using System.Reflection;
using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSidebar;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Sidebar;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;

public class MainViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private ThemeType themeType;

    public string WindowTitle { get; }

    public ThemeType ThemeType
    {
        get => themeType;
        set
        {
            themeType = value;
            OnPropertyChanged(nameof(ThemeType));
        }
    }

    public ToggleThemeCommand ToggleThemeCommand { get; }

    public SidebarViewModel SidebarViewModel { get; }

    public SigningPageViewModel SigningPageViewModel { get; }

    public PluginsPageViewModel PluginsPageViewModel { get; }

    public MainViewModel(IMediator mediator, IEventBus eventBus, SidebarViewModel sidebarViewModel,
        SigningPageViewModel signingPageViewModel, ToggleThemeCommand toggleThemeCommand,
        PluginsPageViewModel pluginsPageViewModel)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        SidebarViewModel = sidebarViewModel ?? throw new ArgumentNullException(nameof(sidebarViewModel));
        SigningPageViewModel = signingPageViewModel ?? throw new ArgumentNullException(nameof(signingPageViewModel));
        ToggleThemeCommand = toggleThemeCommand ?? throw new ArgumentNullException(nameof(toggleThemeCommand));
        PluginsPageViewModel = pluginsPageViewModel ?? throw new ArgumentNullException(nameof(pluginsPageViewModel));

        WindowTitle = GetWindowTitle();

        eventBus.Subscribe<ThemeChangedEvent>(HandleThemeChanged);

        _ = InitializeAsync();
    }

    private static string GetWindowTitle()
    {
        Assembly assembly = Assembly.GetEntryAssembly();
        string version = assembly?.GetName().Version?.ToString(3);
        return $"Crypto Key Management {version}";
    }

    private Task InitializeAsync()
    {
        return AsInitializationAsync(async () =>
        {
            PresentSidebarRequest request = new();
            PresentSidebarResponse response = await mediator.Query<PresentSidebarRequest, PresentSidebarResponse>(request);

            ThemeType = response.ThemeType;
        });
    }

    private Task HandleThemeChanged(ThemeChangedEvent @event, CancellationToken token)
    {
        ThemeType = @event.ThemeType;
        return Task.CompletedTask;
    }
}
