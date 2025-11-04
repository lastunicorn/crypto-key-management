using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Ports.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Watchers;
using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting;
using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting.Contracts;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializeApp;

internal class InitializeAppUseCase : ICommandHandler<InitializeAppRequest>
{
    private readonly ApplicationStateWatcher applicationStateWatcher;
    private readonly IThemeSelector themeSelector;
    private readonly ISettingsService settingsService;
    private readonly IEventBus eventBus;
    private readonly SignatureFormatterPool signatureFormatterPool;

    public InitializeAppUseCase(ApplicationStateWatcher applicationStateWatcher,
        IThemeSelector themeSelector, ISettingsService settingsService,
        IEventBus eventBus, SignatureFormatterPool signatureFormatterPool = null)
    {
        this.applicationStateWatcher = applicationStateWatcher ?? throw new ArgumentNullException(nameof(applicationStateWatcher));
        this.themeSelector = themeSelector ?? throw new ArgumentNullException(nameof(themeSelector));
        this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.signatureFormatterPool = signatureFormatterPool;
    }

    public async Task<ICommandWorkflowResult> Handle(InitializeAppRequest command)
    {
        applicationStateWatcher?.Start();

        ThemeType themeType = settingsService.ThemeType;

        themeSelector.ApplyTheme(themeType);

        ApplySignatureFormatterFromSettings();

        await RaiseThemeChangedEvent(themeType);

        ICommandWorkflowResult result = CommandWorkflowResult.Ok();
        return result;
    }

    private void ApplySignatureFormatterFromSettings()
    {
        if (signatureFormatterPool == null)
            return;

        Guid? formatterId = settingsService.SignatureFormatterId;
        if (formatterId == null)
            return;

        ISignatureFormatter signatureFormatter = signatureFormatterPool.Formatters
            .FirstOrDefault(x => x.Id == formatterId.Value);

        if (signatureFormatter != null)
            signatureFormatterPool.DefaultFormatter = signatureFormatter;
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
