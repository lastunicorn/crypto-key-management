using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SignMessage;

internal class SignMessageUseCase : ICommandHandler<SignMessageRequest>
{
    private readonly IApplicationState applicationState;

    public SignMessageUseCase(IApplicationState applicationState)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public Task<ICommandWorkflowResult> Handle(SignMessageRequest command)
    {
        if (string.IsNullOrWhiteSpace(command.Message))
            throw new ArgumentException("Message cannot be empty", nameof(command.Message));

        SignatureKey signatureKey = applicationState.CurrentSignatureKey;

        if (signatureKey == null)
            throw new InvalidOperationException("No signature key selected");

        string signature = SignTheMessage(signatureKey, command.Message);

        SignMessageResponse response = new()
        {
            Message = command.Message,
            Signature = signature
        };

        ICommandWorkflowResult result = new CommandWorkflowResult<SignMessageResponse>(response);
        return Task.FromResult(result);
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