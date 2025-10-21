using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace DustInTheWind.SignatureManagement.Application.CreateSignature;

internal class CreateSignatureUseCase : ICommandHandler<CreateSignatureCommand>
{
    private readonly ISignatureKeyRepository signatureRepository;

    public CreateSignatureUseCase(ISignatureKeyRepository signatureRepository)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
    }

    public Task<ICommandWorkflowResult> Handle(CreateSignatureCommand command)
    {
        Console.WriteLine("Creating new Ed25519 key pair...");

        // Generate new key pair
        Ed25519KeyPairGenerator keyPairGenerator = new();
        keyPairGenerator.Init(new Ed25519KeyGenerationParameters(new SecureRandom()));
        AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();

        Ed25519PrivateKeyParameters privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;
        Ed25519PublicKeyParameters publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

        Guid signatureId = signatureRepository.Add(privateKey, publicKey);

        // Retrieve the saved signature to get file paths
        SignatureKey savedSignature = signatureRepository.GetById(signatureId);

        CreateSignatureResponse response = new()
        {
            KeyId = signatureId,
            PrivateKeyPath = savedSignature.PrivateKeyPath,
            PublicKeyPath = savedSignature.PublicKeyPath,
            PrivateKey = savedSignature.PrivateKey,
            PublicKey = savedSignature.PublicKey
        };

        ICommandWorkflowResult result = new CommandWorkflowResult<CreateSignatureResponse>(response);
        return Task.FromResult(result);
    }
}