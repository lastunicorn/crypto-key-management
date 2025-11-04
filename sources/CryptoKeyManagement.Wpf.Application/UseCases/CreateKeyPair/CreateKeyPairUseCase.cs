using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.CreateKeyPair;

internal class CreateKeyPairUseCase : ICommandHandler<CreateKeyPairRequest>
{
    private readonly ICryptoKeyRepository cryptoKeyRepository;
    private readonly IEventBus eventBus;

    public CreateKeyPairUseCase(ICryptoKeyRepository cryptoKeyRepository, IEventBus eventBus)
    {
        this.cryptoKeyRepository = cryptoKeyRepository ?? throw new ArgumentNullException(nameof(cryptoKeyRepository));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<ICommandWorkflowResult> Handle(CreateKeyPairRequest command)
    {
        AsymmetricCipherKeyPair keyPair = GenerateNewKeyPair();

        Ed25519PrivateKeyParameters privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;
        Ed25519PublicKeyParameters publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

        Guid signatureKeyId = cryptoKeyRepository.Add(privateKey.GetEncoded(), publicKey.GetEncoded());
        KeyPair savedSignatureKey = cryptoKeyRepository.GetById(signatureKeyId);

        KeyPairCreatedEvent @event = new()
        {
            SignatureKey = savedSignatureKey.ToDto()
        };
        await eventBus.PublishAsync(@event);

        return CommandWorkflowResult.Ok();
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