using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DustInTheWind.SignatureManagement.Application.SignData;

internal class SignDataUseCase : IQuery<SignDataCriteria, SignDataResult>
{
    private readonly ISignatureRepository signatureRepository;

    public SignDataUseCase(ISignatureRepository signatureRepository)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
    }

    public Task<SignDataResult> Query(SignDataCriteria criteria)
    {
        var signatures = signatureRepository.GetAvailableSignatures();

        if (!signatures.Any())
            throw new NoSignaturesException();

        // Show available signatures
        Console.WriteLine("Available Signatures:");
        foreach (var sig in signatures)
            Console.WriteLine($"- {sig.Id}");

        // Get signature ID from user
        Console.Write("\nEnter Signature ID (GUID): ");
        string signatureIdInput = Console.ReadLine()?.Trim();

        if (!Guid.TryParse(signatureIdInput, out Guid signatureId))
            throw new InvalidSignatureIdException(signatureIdInput);

        var selectedSignature = signatures.FirstOrDefault(s => s.Id == signatureId);
        if (selectedSignature == null)
            throw new InvalidSignatureIdException(signatureIdInput);

        // Get data to sign
        Console.Write("Enter data to sign: ");
        string dataToSign = Console.ReadLine();

        if (string.IsNullOrEmpty(dataToSign))
            throw new NoDataToSignException();

        // Load private key
        string privateKeyBase64 = File.ReadAllText(selectedSignature.PrivateKeyPath);
        byte[] privateKeyBytes = Convert.FromBase64String(privateKeyBase64);
        Ed25519PrivateKeyParameters privateKey = new Ed25519PrivateKeyParameters(privateKeyBytes, 0);

        // Sign the data
        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(dataToSign);
        Ed25519Signer signer = new Ed25519Signer();
        signer.Init(true, privateKey);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        byte[] signature = signer.GenerateSignature();

        Console.WriteLine("\n✓ Data signed successfully!");
        Console.WriteLine($"Original Data: {dataToSign}");
        Console.WriteLine($"Signature ID: {signatureId}");
        Console.WriteLine($"Signed Data (Base64): {Convert.ToBase64String(signature)}\n");

        SignDataResult result = new SignDataResult
        {
            SignatureId = signatureId,
            OriginalData = dataToSign,
            SignedDataBase64 = Convert.ToBase64String(signature)
        };

        return Task.FromResult(result);
    }
}