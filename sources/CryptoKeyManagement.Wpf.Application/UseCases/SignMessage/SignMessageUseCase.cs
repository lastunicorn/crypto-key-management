using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptographyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SignMessage;

internal class SignMessageUseCase : ICommandHandler<SignMessageRequest>
{
    private readonly IApplicationState applicationState;
    private readonly IEventBus eventBus;
    private readonly ICryptographyService cryptographyService;

    public SignMessageUseCase(IApplicationState applicationState, IEventBus eventBus, ICryptographyService cryptographyService)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
    }

    public async Task<ICommandWorkflowResult> Handle(SignMessageRequest command)
    {
        if (string.IsNullOrWhiteSpace(command.Message))
            throw new ArgumentException("Message cannot be empty", nameof(command.Message));

        KeyPair keyPair = applicationState.CurrentSignatureKey;

        if (keyPair == null)
            throw new InvalidOperationException("No signature key selected");

        byte[] signatureBytes = cryptographyService.Sign(keyPair, command.Message);

        // Store the signature and message in application state
        applicationState.CurrentMessage = command.Message;
        applicationState.CurrentSignature = signatureBytes;

        // Publish event to notify interested parties
        await PublishSignatureChangedEvent(command.Message, signatureBytes, keyPair);

        SignMessageResponse response = new()
        {
            Message = command.Message,
            Signature = signatureBytes
        };

        return new CommandWorkflowResult<SignMessageResponse>(response);
    }

    private async Task PublishSignatureChangedEvent(string message, byte[] signature, KeyPair keyPair)
    {
        SignatureCreatedEvent @event = new()
        {
            Message = message,
            Signature = signature,
            KeyPair = keyPair
        };
        await eventBus.PublishAsync(@event);
    }
}