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

        Guid signatureId = signatureRepository.SaveSignatureKey(privateKey, publicKey);

        // Retrieve the saved signature to get file paths
        SignatureKeyInfo savedSignature = signatureRepository.GetSignatureById(signatureId);

        Console.WriteLine($"✓ Signature created successfully!");
        Console.WriteLine($"  Signature ID: {signatureId}");
        Console.WriteLine($"  Private Key saved to: {savedSignature.PrivateKeyPath}");
        Console.WriteLine($"  Public Key saved to: {savedSignature.PublicKeyPath}");
        Console.WriteLine($"  Private Key Length: {savedSignature.PrivateKey.Length} bytes");
        Console.WriteLine($"  Public Key Length: {savedSignature.PublicKey.Length} bytes\n");

        ICommandWorkflowResult result = CommandWorkflowResult.Ok();
        return Task.FromResult(result);
    }
}