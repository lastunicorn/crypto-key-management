using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.SelectSignatureKey;

internal class SelectSignatureKeyUseCase : ICommandHandler<SelectSignatureKeyRequest>
{
    private readonly ISignatureKeyRepository signatureKeyRepository;
    private readonly IApplicationState applicationStateService;
    private readonly EventBus eventBus;

    public SelectSignatureKeyUseCase(ISignatureKeyRepository signatureKeyRepository, IApplicationState applicationStateService, EventBus eventBus)
    {
        this.signatureKeyRepository = signatureKeyRepository ?? throw new ArgumentNullException(nameof(signatureKeyRepository));
        this.applicationStateService = applicationStateService ?? throw new ArgumentNullException(nameof(applicationStateService));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<ICommandWorkflowResult> Handle(SelectSignatureKeyRequest command)
    {
        SignatureKey signatureKey = RetrieveSignatureKey(command.SignatureKeyId);
        applicationStateService.SelectedSignatureKeyId = signatureKey?.Id;

        await RaiseSignatureKeySelectionChangedEvent(signatureKey);

        return CommandWorkflowResult.Ok();
    }

    private async Task RaiseSignatureKeySelectionChangedEvent(SignatureKey signatureKey)
    {
        SignatureKeySelectionChangedEvent @event = new()
        {
            SelectedKey = signatureKey.ToDto()
        };
        await eventBus.PublishAsync(@event);
    }

    private SignatureKey RetrieveSignatureKey(Guid? signatureKeyId)
    {
        return signatureKeyId.HasValue
            ? signatureKeyRepository.GetById(signatureKeyId.Value)
            : null;
    }
}