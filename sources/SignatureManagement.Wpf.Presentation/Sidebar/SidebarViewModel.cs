using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Sidebar;

public class SidebarViewModel : ViewModelBase
{
    private string themeToggleText = "🌜";

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

    public SidebarViewModel(EventBus eventBus, ToggleThemeCommand toggleThemeCommand)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        ToggleThemeCommand = toggleThemeCommand ?? throw new ArgumentNullException(nameof(toggleThemeCommand));

        eventBus.Subscribe<ThemeChangedEvent>(HandleThemeChanged);
    }

    private Task HandleThemeChanged(ThemeChangedEvent @event, CancellationToken token)
    {
        ThemeToggleText = @event.ThemeType switch
        {
            ThemeType.Light => "🌞",
            ThemeType.Dark => "🌜",
            _ => string.Empty
        };


        return Task.CompletedTask;
    }
}