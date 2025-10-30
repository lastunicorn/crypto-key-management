using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.UserAccess;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DustInTheWind.SignatureManagement.Application.SignData;

internal class SignDataUseCase : IQuery<SignDataCriteria, SignDataResponse>
{
    private readonly ISignatureKeyRepository signatureRepository;
    private readonly IUserConsole userConsole;

    public SignDataUseCase(ISignatureKeyRepository signatureRepository, IUserConsole userConsole)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
        this.userConsole = userConsole ?? throw new ArgumentNullException(nameof(userConsole));
    }

    public Task<SignDataResponse> Query(SignDataCriteria criteria)
    {
        List<SignatureKey> signatures = GetAllSignatures();
        DisplaySignatures(signatures);

        SignatureKey selectedSignature = AskForSignatureToUse(signatures);

        string dataToSign = userConsole.AskForDataToSign();
        byte[] signature = SignTheData(selectedSignature, dataToSign);

        SignDataResponse result = new()
        {
            Signature = signature
        };

        return Task.FromResult(result);
    }

    private List<SignatureKey> GetAllSignatures()
    {
        List<SignatureKey> signatures = signatureRepository.GetAll()
            .ToList();

        if (signatures.Count == 0)
            throw new NoKeysException();

        return signatures;
    }

    private void DisplaySignatures(List<SignatureKey> signatures)
    {
        IEnumerable<SignatureSummary> signatureSummaries = signatures
            .Select(x => new SignatureSummary
            {
                Id = x.Id,
                PrivateKey = x.PrivateKey,
                PublicKey = x.PublicKey,
                CreatedDate = x.CreatedDate
            });

        userConsole.DisplaySignatures(signatureSummaries);
    }

    private SignatureKey AskForSignatureToUse(List<SignatureKey> signatures)
    {
        Guid? signatureId = userConsole.AskSignatureId();

        if (!signatureId.HasValue)
            throw new InvalidSignatureIdException("Invalid GUID format");

        SignatureKey selectedSignature = signatures.FirstOrDefault(x => x.Id == signatureId.Value);

        return selectedSignature == null
            ? throw new InvalidSignatureIdException(signatureId.Value.ToString())
            : selectedSignature;
    }

    private static byte[] SignTheData(SignatureKey selectedSignature, string dataToSign)
    {
        Ed25519PrivateKeyParameters privateKey = new(selectedSignature.PrivateKey, 0);

        byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(dataToSign);
        Ed25519Signer signer = new();
        signer.Init(true, privateKey);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        return signer.GenerateSignature();
    }
}