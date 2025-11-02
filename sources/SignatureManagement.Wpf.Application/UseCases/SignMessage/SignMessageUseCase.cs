using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.CryptographyAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SignMessage;

internal class SignMessageUseCase : ICommandHandler<SignMessageRequest>
{
    private readonly IApplicationState applicationState;
    private readonly EventBus eventBus;
    private readonly ICryptographyService cryptographyService;

    public SignMessageUseCase(IApplicationState applicationState, EventBus eventBus, ICryptographyService cryptographyService)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
    }

    public async Task<ICommandWorkflowResult> Handle(SignMessageRequest command)
    {
        if (string.IsNullOrWhiteSpace(command.Message))
            throw new ArgumentException("Message cannot be empty", nameof(command.Message));

        KeyPair signatureKey = applicationState.CurrentSignatureKey;

        if (signatureKey == null)
            throw new InvalidOperationException("No signature key selected");

        byte[] signatureBytes = cryptographyService.Sign(signatureKey, command.Message);

        // Store the signature and message in application state
        applicationState.CurrentMessage = command.Message;
        applicationState.CurrentSignature = signatureBytes;

        // Publish event to notify interested parties
        await PublishSignatureChangedEvent(command.Message, signatureBytes);

        SignMessageResponse response = new()
        {
            Message = command.Message,
            Signature = signatureBytes
        };

        return new CommandWorkflowResult<SignMessageResponse>(response);
    }

    private async Task PublishSignatureChangedEvent(string message, byte[] signature)
    {
        SignatureCreatedEvent @event = new()
        {
            Message = message,
            Signature = signature
        };
        await eventBus.PublishAsync(@event);
    }
}