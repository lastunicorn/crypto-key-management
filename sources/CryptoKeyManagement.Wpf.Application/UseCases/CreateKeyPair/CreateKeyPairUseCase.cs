using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.CreateKeyPair;

internal class CreateKeyPairUseCase : ICommandHandler<CreateKeyPairRequest>
{
    private readonly ISignatureKeyRepository signatureRepository;
    private readonly IEventBus eventBus;

    public CreateKeyPairUseCase(ISignatureKeyRepository signatureRepository, IEventBus eventBus)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<ICommandWorkflowResult> Handle(CreateKeyPairRequest command)
    {
        AsymmetricCipherKeyPair keyPair = GenerateNewKeyPair();

        Ed25519PrivateKeyParameters privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;
        Ed25519PublicKeyParameters publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

        Guid signatureKeyId = signatureRepository.Add(privateKey.GetEncoded(), publicKey.GetEncoded());
        KeyPair savedSignatureKey = signatureRepository.GetById(signatureKeyId);

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