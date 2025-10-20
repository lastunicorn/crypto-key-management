using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace DustInTheWind.SignatureManagement.Application.CreateSignature;

internal class CreateSignatureUseCase : ICommandHandler<CreateSignatureCommand>
{
    private readonly ISignatureRepository signatureRepository;

    public CreateSignatureUseCase(ISignatureRepository signatureRepository)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
    }

    public Task<ICommandWorkflowResult> Handle(CreateSignatureCommand command)
    {
        Console.WriteLine("Creating new Ed25519 key pair...");

        // Generate new key pair
        Ed25519KeyPairGenerator keyPairGenerator = new Ed25519KeyPairGenerator();
        keyPairGenerator.Init(new Ed25519KeyGenerationParameters(new SecureRandom()));
        AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();

        Ed25519PrivateKeyParameters privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;
        Ed25519PublicKeyParameters publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

        Guid signatureId = signatureRepository.SaveSignatureKey(privateKey, publicKey, out string privateKeyPath, out string publicKeyPath);

        Console.WriteLine($"✓ Signature created successfully!");
        Console.WriteLine($"  Signature ID: {signatureId}");
        Console.WriteLine($"  Private Key saved to: {privateKeyPath}");
        Console.WriteLine($"  Public Key saved to: {publicKeyPath}");
        Console.WriteLine($"  Private Key Length: {privateKey.GetEncoded().Length} bytes");
        Console.WriteLine($"  Public Key Length: {publicKey.GetEncoded().Length} bytes\n");

        ICommandWorkflowResult result = CommandWorkflowResult.Ok();
        return Task.FromResult(result);
    }
}