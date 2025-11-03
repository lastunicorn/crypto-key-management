using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SelectKeyPair;

internal class SelectKeyPairUseCase : ICommandHandler<SelectKeyPairRequest>
{
    private readonly ISignatureKeyRepository signatureKeyRepository;
    private readonly IApplicationState applicationStateService;
    private readonly IEventBus eventBus;

    public SelectKeyPairUseCase(ISignatureKeyRepository signatureKeyRepository, IApplicationState applicationStateService, IEventBus eventBus)
    {
        this.signatureKeyRepository = signatureKeyRepository ?? throw new ArgumentNullException(nameof(signatureKeyRepository));
        this.applicationStateService = applicationStateService ?? throw new ArgumentNullException(nameof(applicationStateService));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<ICommandWorkflowResult> Handle(SelectKeyPairRequest command)
    {
        KeyPair signatureKey = RetrieveSignatureKey(command.SignatureKeyId);
        applicationStateService.CurrentSignatureKey = signatureKey;

        await RaiseSignatureKeySelectionChangedEvent(signatureKey);

        return CommandWorkflowResult.Ok();
    }

    private async Task RaiseSignatureKeySelectionChangedEvent(KeyPair signatureKey)
    {
        KeyPairSelectionChangedEvent @event = new()
        {
            SignatureKey = signatureKey.ToDto()
        };
        await eventBus.PublishAsync(@event);
    }

    private KeyPair RetrieveSignatureKey(Guid? signatureKeyId)
    {
        return signatureKeyId.HasValue
            ? signatureKeyRepository.GetById(signatureKeyId.Value)
            : null;
    }
}