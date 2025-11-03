using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.DeleteKeyPair;

internal class DeleteKeyPairUseCase : ICommandHandler<DeleteKeyPairRequest>
{
    private readonly ISignatureKeyRepository signatureKeyRepository;
    private readonly IApplicationState applicationState;
    private readonly IEventBus eventBus;

    public DeleteKeyPairUseCase(
        ISignatureKeyRepository signatureKeyRepository,
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