using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.DeleteKeyPair;

internal class DeleteKeyPairUseCase : ICommandHandler<DeleteKeyPairRequest>
{
    private readonly ICryptoKeyRepository signatureKeyRepository;
    private readonly IApplicationState applicationState;
    private readonly IEventBus eventBus;

    public DeleteKeyPairUseCase(
        ICryptoKeyRepository signatureKeyRepository,
        IApplicationState applicationState,
        IEventBus eventBus)
    {
        this.signatureKeyRepository = signatureKeyRepository ?? throw new ArgumentNullException(nameof(signatureKeyRepository));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<ICommandWorkflowResult> Handle(DeleteKeyPairRequest command)
    {
        ValidateRequest(command);

        signatureKeyRepository.Delete(command.KeyPairId);
        ClearFromState(command.KeyPairId);
        await PublishKeyPairDeletedEvent(command.KeyPairId);

        return new CommandWorkflowResult();
    }

    private static void ValidateRequest(DeleteKeyPairRequest command)
    {
    }

    private void ClearFromState(Guid keyPairId)
    {
        bool isSelectedKey = applicationState.CurrentSignatureKey?.Id == keyPairId;

        if (isSelectedKey)
        {
            applicationState.CurrentSignatureKey = null;
            applicationState.CurrentMessage = null;
            applicationState.CurrentSignature = null;
        }
    }

    private async Task PublishKeyPairDeletedEvent(Guid keyPairId)
    {
        KeyPairDeletedEvent @event = new()
        {
            KeyPairId = keyPairId
        };
        await eventBus.PublishAsync(@event);
    }
}