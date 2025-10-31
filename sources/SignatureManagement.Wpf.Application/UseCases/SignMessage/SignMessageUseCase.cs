using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SignMessage;

internal class SignMessageUseCase : ICommandHandler<SignMessageRequest>
{
    private readonly IApplicationState applicationState;
    private readonly EventBus eventBus;

    public SignMessageUseCase(IApplicationState applicationState, EventBus eventBus)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<ICommandWorkflowResult> Handle(SignMessageRequest command)
    {
        if (string.IsNullOrWhiteSpace(command.Message))
            throw new ArgumentException("Message cannot be empty", nameof(command.Message));

        SignatureKey signatureKey = applicationState.CurrentSignatureKey;

        if (signatureKey == null)
            throw new InvalidOperationException("No signature key selected");

        string signature = SignTheMessage(signatureKey, command.Message);

        // Store the signature and message in application state
        applicationState.CurrentMessage = command.Message;
        applicationState.CurrentSignature = signature;

        // Publish event to notify interested parties
        await PublishSignatureChangedEvent(command.Message, signature);

        SignMessageResponse response = new()
        {
            Message = command.Message,
            Signature = signature
        };

        return new CommandWorkflowResult<SignMessageResponse>(response);
    }

    private async Task PublishSignatureChangedEvent(string message, string signature)
    {
        SignatureChangedEvent @event = new()
        {
            Message = message,
            Signature = signature
        };
        await eventBus.PublishAsync(@event);
    }

    private static string SignTheMessage(SignatureKey selectedSignature, string message)
    {
        Ed25519PrivateKeyParameters privateKey = new(selectedSignature.PrivateKey, 0);

        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
        Ed25519Signer signer = new();
        signer.Init(true, privateKey);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        byte[] signatureBytes = signer.GenerateSignature();

        return Convert.ToBase64String(signatureBytes);
    }
}