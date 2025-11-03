using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Ports.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.ToggleTheme;

internal class ToggleThemeUseCase : ICommandHandler<ToggleThemeRequest>
{
    private readonly ISettingsService settingsService;
    private readonly IEventBus eventBus;
    private readonly IThemeSelector themeSelector;

    public ToggleThemeUseCase(ISettingsService settingsService, IEventBus eventBus, IThemeSelector themeSelector)
    {
        this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.themeSelector = themeSelector ?? throw new ArgumentNullException(nameof(themeSelector));
    }

    public async Task<ICommandWorkflowResult> Handle(ToggleThemeRequest command)
    {
        ThemeType newThemeType = settingsService.ThemeType switch
        {
            ThemeType.Light => ThemeType.Dark,
            ThemeType.Dark => ThemeType.Light,
            _ => ThemeType.Light
        };

        settingsService.ThemeType = newThemeType;
        themeSelector.ApplyTheme(newThemeType);

        await RaiseThemeChangedEvent(newThemeType);

        return CommandWorkflowResult.Ok();
    }

    private async Task RaiseThemeChangedEvent(ThemeType newThemeType)
    {
        ThemeChangedEvent themeChangedEvent = new()
        {
            ThemeType = newThemeType
        };

        await eventBus.PublishAsync(themeChangedEvent);
    }
}