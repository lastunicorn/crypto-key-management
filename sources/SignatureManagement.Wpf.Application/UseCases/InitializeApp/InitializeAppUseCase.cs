using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SettingsAccess;
using DustInTheWind.SignatureManagement.Ports.WpfUserAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.Watchers;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeApp;

internal class InitializeAppUseCase : ICommandHandler<InitializeAppRequest>
{
    private readonly ApplicationStateWatcher applicationStateWatcher;
    private readonly IThemeSelector themeSelector;
    private readonly ISettingsService settingsService;
    private readonly EventBus eventBus;

    public InitializeAppUseCase(ApplicationStateWatcher applicationStateWatcher,
        IThemeSelector themeSelector, ISettingsService settingsService,
        EventBus eventBus)
    {
        this.applicationStateWatcher = applicationStateWatcher ?? throw new ArgumentNullException(nameof(applicationStateWatcher));
        this.themeSelector = themeSelector ?? throw new ArgumentNullException(nameof(themeSelector));
        this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<ICommandWorkflowResult> Handle(InitializeAppRequest command)
    {
        applicationStateWatcher?.Start();

        ThemeType themeType = settingsService.ThemeType;

        themeSelector.ApplyTheme(themeType);

        await RaiseThemeChangedEvent(themeType);

        ICommandWorkflowResult result = CommandWorkflowResult.Ok();
        return result;
    }

    private async Task RaiseThemeChangedEvent(ThemeType themeType)
    {
        ThemeChangedEvent themeChangedEvent = new()
        {
            ThemeType = themeType
        };

        await eventBus.PublishAsync(themeChangedEvent);
    }
}
