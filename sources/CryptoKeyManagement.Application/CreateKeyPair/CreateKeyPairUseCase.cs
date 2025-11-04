using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace DustInTheWind.CryptoKeyManagement.Application.CreateKeyPair;

internal class CreateKeyPairUseCase : ICommandHandler<CreateKeyPairRequest>
{
    private readonly ICryptoKeyRepository cryptoKeyRepository;

    public CreateKeyPairUseCase(ICryptoKeyRepository cryptoKeyRepository)
    {
        this.cryptoKeyRepository = cryptoKeyRepository ?? throw new ArgumentNullException(nameof(cryptoKeyRepository));
    }

    public Task<ICommandWorkflowResult> Handle(CreateKeyPairRequest command)
    {
        // Generate new key pair
        AsymmetricCipherKeyPair keyPair = GenerateNewKeyPair();

        Ed25519PrivateKeyParameters privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;
        Ed25519PublicKeyParameters publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

        Guid signatureKeyId = cryptoKeyRepository.Add(privateKey.GetEncoded(), publicKey.GetEncoded());
        KeyPair savedKeyPair = cryptoKeyRepository.GetById(signatureKeyId);

        CreateKeyPairResponse response = new()
        {
            KeyId = signatureKeyId,
            PrivateKeyPath = savedKeyPair.PrivateKeyPath,
            PublicKeyPath = savedKeyPair.PublicKeyPath,
            PrivateKey = savedKeyPair.PrivateKey,
            PublicKey = savedKeyPair.PublicKey
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