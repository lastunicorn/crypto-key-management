using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.UserAccess;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DustInTheWind.SignatureManagement.Application.SignData;

internal class SignDataUseCase : IQuery<SignDataCriteria, SignDataResponse>
{
    private readonly ISignatureRepository signatureRepository;
    private readonly IUserConsole userConsole;

    public SignDataUseCase(ISignatureRepository signatureRepository, IUserConsole userConsole)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
        this.userConsole = userConsole ?? throw new ArgumentNullException(nameof(userConsole));
    }

    public Task<SignDataResponse> Query(SignDataCriteria criteria)
    {
        List<SignatureKeyInfo> signatures = signatureRepository.GetAll()
            .ToList();

        if (!signatures.Any())
            throw new NoSignaturesException();

        DisplaySignatures(signatures);

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

        SignDataResponse result = new()
        {
            SignatureId = signatureId,
            Signature = signature
        };

        return Task.FromResult(result);
    }

    private void DisplaySignatures(List<SignatureKeyInfo> signatures)
    {
        IEnumerable<SignatureSummary> signatureSummaries = signatures
            .Select(x => new SignatureSummary
            {
                Id = x.Id,
                CreatedDate = x.CreatedDate
            });

        userConsole.DisplaySignatures(signatureSummaries);
    }
}