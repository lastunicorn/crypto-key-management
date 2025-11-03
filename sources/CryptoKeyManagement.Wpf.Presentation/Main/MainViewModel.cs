using System.Reflection;
using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSidebar;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Sidebar;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;

public class MainViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private string themeToggleText;

    public string WindowTitle { get; }

    public string ThemeToggleText
    {
        get => themeToggleText;
        private set
        {
            themeToggleText = value;
            OnPropertyChanged(nameof(ThemeToggleText));
        }
    }

    public ToggleThemeCommand ToggleThemeCommand { get; }

    public SidebarViewModel SidebarViewModel { get; }

    public SigningPageViewModel SigningPageViewModel { get; }

    public MainViewModel(IMediator mediator, IEventBus eventBus, SidebarViewModel sidebarViewModel,
        SigningPageViewModel signingPageViewModel, ToggleThemeCommand toggleThemeCommand)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        SidebarViewModel = sidebarViewModel ?? throw new ArgumentNullException(nameof(sidebarViewModel));
        SigningPageViewModel = signingPageViewModel ?? throw new ArgumentNullException(nameof(signingPageViewModel));
        ToggleThemeCommand = toggleThemeCommand ?? throw new ArgumentNullException(nameof(toggleThemeCommand));

        WindowTitle = GetWindowTitle();

        eventBus.Subscribe<ThemeChangedEvent>(HandleThemeChanged);

        _ = InitializeAsync();
    }

    private static string GetWindowTitle()
    {
        Assembly assembly = Assembly.GetEntryAssembly();
        string version = assembly?.GetName().Version?.ToString(3) ?? "Unknown";
        return $"Crypto Key Management {version}";
    }

    private Task InitializeAsync()
    {
        return AsInitializationAsync(async () =>
        {
            PresentSidebarRequest request = new();
            PresentSidebarResponse response = await mediator.Query<PresentSidebarRequest, PresentSidebarResponse>(request);

            UpdateThemeToggleText(response.ThemeType);
        });
    }

    private Task HandleThemeChanged(ThemeChangedEvent @event, CancellationToken token)
    {
        UpdateThemeToggleText(@event.ThemeType);
        return Task.CompletedTask;
    }

    private void UpdateThemeToggleText(ThemeType themeType)
    {
        ThemeToggleText = themeType switch
        {
            ThemeType.Light => "🌞",
            ThemeType.Dark => "🌜",
            _ => string.Empty
        };
    }
}
