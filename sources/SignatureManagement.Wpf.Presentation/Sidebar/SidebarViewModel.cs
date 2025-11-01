using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentSidebar;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Sidebar;

public class SidebarViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private string themeToggleText;

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

    public SidebarViewModel(IMediator mediator, EventBus eventBus, ToggleThemeCommand toggleThemeCommand)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        ArgumentNullException.ThrowIfNull(eventBus);
        ToggleThemeCommand = toggleThemeCommand ?? throw new ArgumentNullException(nameof(toggleThemeCommand));

        eventBus.Subscribe<ThemeChangedEvent>(HandleThemeChanged);

        _ = InitializeAsync();
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