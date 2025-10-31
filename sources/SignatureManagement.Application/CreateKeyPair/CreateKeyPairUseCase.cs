using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace DustInTheWind.SignatureManagement.Application.CreateKeyPair;

internal class CreateKeyPairUseCase : ICommandHandler<CreateKeyPairRequest>
{
    private readonly ISignatureKeyRepository signatureRepository;

    public CreateKeyPairUseCase(ISignatureKeyRepository signatureRepository)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
    }

    public Task<ICommandWorkflowResult> Handle(CreateKeyPairRequest command)
    {
        // Generate new key pair
        AsymmetricCipherKeyPair keyPair = GenerateNewKeyPair();

        Ed25519PrivateKeyParameters privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;
        Ed25519PublicKeyParameters publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

        Guid signatureKeyId = signatureRepository.Add(privateKey.GetEncoded(), publicKey.GetEncoded());
        KeyPair savedSignatureKey = signatureRepository.GetById(signatureKeyId);

        CreateKeyPairResponse response = new()
        {
            KeyId = signatureKeyId,
            PrivateKeyPath = savedSignatureKey.PrivateKeyPath,
            PublicKeyPath = savedSignatureKey.PublicKeyPath,
            PrivateKey = savedSignatureKey.PrivateKey,
            PublicKey = savedSignatureKey.PublicKey
        };

        ICommandWorkflowResult result = new CommandWorkflowResult<CreateKeyPairResponse>(response);
        return Task.FromResult(result);
    }

    private static AsymmetricCipherKeyPair GenerateNewKeyPair()
    {
        Ed25519KeyPairGenerator keyPairGenerator = new();

        SecureRandom secureRandom = new();
        Ed25519KeyGenerationParameters parameters = new(secureRandom);
        keyPairGenerator.Init(parameters);

        return keyPairGenerator.GenerateKeyPair();
    }
}