using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SelectKeyPair;

internal class SelectKeyPairUseCase : ICommandHandler<SelectKeyPairRequest>
{
    private readonly ISignatureKeyRepository signatureKeyRepository;
    private readonly IApplicationState applicationStateService;
    private readonly EventBus eventBus;

    public SelectKeyPairUseCase(ISignatureKeyRepository signatureKeyRepository, IApplicationState applicationStateService, EventBus eventBus)
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