using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DustInTheWind.SignatureManagement.Wpf.Application.SignMessage;

internal class SignMessageUseCase : ICommandHandler<SignMessageRequest>
{
    private readonly ISignatureKeyRepository signatureRepository;
    private readonly IApplicationState applicationState;

    public SignMessageUseCase(ISignatureKeyRepository signatureRepository, IApplicationState applicationState)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public Task<ICommandWorkflowResult> Handle(SignMessageRequest command)
    {
        if (string.IsNullOrWhiteSpace(command.Message))
            throw new ArgumentException("Message cannot be empty", nameof(command.Message));

        Guid? signatureKeyId = applicationState.SelectedSignatureKeyId;

        if (!signatureKeyId.HasValue)
            throw new InvalidOperationException("No signature key selected");

        SignatureKey signatureKey = signatureRepository.GetById(signatureKeyId.Value);
        if (signatureKey == null)
            throw new InvalidOperationException($"Signature key with ID {signatureKeyId.Value} not found");

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