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
        List<SignatureKeyInfo> signatures = signatureRepository.GetAvailableSignatures();

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

        SignatureKeyInfo selectedSignature = signatures.FirstOrDefault(s => s.Id == signatureId);
        if (selectedSignature == null)
            throw new InvalidSignatureIdException(signatureIdInput);

        // Get data to sign
        Console.Write("Enter data to sign: ");
        string dataToSign = Console.ReadLine();

        if (string.IsNullOrEmpty(dataToSign))
            throw new NoDataToSignException();

        // Load private key
        Ed25519PrivateKeyParameters privateKey = new(selectedSignature.PrivateKey, 0);

        // Sign the data
        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(dataToSign);
        Ed25519Signer signer = new();
        signer.Init(true, privateKey);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        byte[] signature = signer.GenerateSignature();

        SignDataResult result = new()
        {
            SignatureId = signatureId,
            OriginalData = dataToSign,
            Signature = signature
        };

        return Task.FromResult(result);
    }
}