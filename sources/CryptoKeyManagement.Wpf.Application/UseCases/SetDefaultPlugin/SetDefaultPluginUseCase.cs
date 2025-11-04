using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SetDefaultPlugin;

internal class SetDefaultPluginUseCase : ICommandHandler<SetDefaultPluginRequest>
{
    private readonly SignatureFormatterPool signatureFormatterPool;
    private readonly IEventBus eventBus;
    private readonly ISettingsService settingsService;

    public SetDefaultPluginUseCase(SignatureFormatterPool signatureFormatterPool, IEventBus eventBus, ISettingsService settingsService)
    {
        this.signatureFormatterPool = signatureFormatterPool ?? throw new ArgumentNullException(nameof(signatureFormatterPool));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
    }

    public async Task<ICommandWorkflowResult> Handle(SetDefaultPluginRequest command)
    {
        bool success = signatureFormatterPool.ChooseDefaultFormatter(x => x.Id == command.PluginId);

        if (!success)
            throw new PluginNotFoundException(command.PluginId);

        settingsService.SignatureFormatterId = signatureFormatterPool.DefaultFormatter.Id;

        await RaiseDefaultPluginChangedEvent();

        return CommandWorkflowResult.Ok();
    }

    private async Task RaiseDefaultPluginChangedEvent()
    {
        DefaultPluginChangedEvent @event = new()
        {
            NewDefaultPluginId = signatureFormatterPool.DefaultFormatter?.Id
        };
        await eventBus.PublishAsync(@event);
    }
}