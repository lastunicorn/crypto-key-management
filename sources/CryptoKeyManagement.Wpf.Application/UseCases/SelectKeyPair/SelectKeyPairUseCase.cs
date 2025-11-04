using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SelectKeyPair;

internal class SelectKeyPairUseCase : ICommandHandler<SelectKeyPairRequest>
{
    private readonly ICryptoKeyRepository cryptoKeyRepository;
    private readonly IApplicationState applicationStateService;
    private readonly IEventBus eventBus;

    public SelectKeyPairUseCase(ICryptoKeyRepository cryptoKeyRepository, IApplicationState applicationStateService, IEventBus eventBus)
    {
        this.cryptoKeyRepository = cryptoKeyRepository ?? throw new ArgumentNullException(nameof(cryptoKeyRepository));
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
            ? cryptoKeyRepository.GetById(signatureKeyId.Value)
            : null;
    }
}