using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.CryptographyAccess;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.UserAccess;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DustInTheWind.SignatureManagement.Application.SignData;

internal class SignDataUseCase : IQuery<SignDataCriteria, SignDataResponse>
{
    private readonly ISignatureKeyRepository signatureRepository;
    private readonly IUserConsole userConsole;
    private readonly ICryptographyService cryptographyService;

    public SignDataUseCase(ISignatureKeyRepository signatureRepository, IUserConsole userConsole, ICryptographyService cryptographyService)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
        this.userConsole = userConsole ?? throw new ArgumentNullException(nameof(userConsole));
        this.cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
    }

    public Task<SignDataResponse> Query(SignDataCriteria criteria)
    {
        List<KeyPair> signatures = GetAllSignatures();
        DisplaySignatures(signatures);

        KeyPair selectedSignature = AskForSignatureToUse(signatures);

        string dataToSign = userConsole.AskForDataToSign();
        byte[] signature = cryptographyService.Sign(selectedSignature, dataToSign);

        SignDataResponse result = new()
        {
            Signature = signature
        };

        return Task.FromResult(result);
    }

    private List<KeyPair> GetAllSignatures()
    {
        List<KeyPair> signatures = signatureRepository.GetAll()
            .ToList();

        if (signatures.Count == 0)
            throw new NoKeysException();

        return signatures;
    }

    private void DisplaySignatures(List<KeyPair> signatures)
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

    private KeyPair AskForSignatureToUse(List<KeyPair> signatures)
    {
        Guid? signatureId = userConsole.AskSignatureId();

        if (!signatureId.HasValue)
            throw new InvalidSignatureIdException("Invalid GUID format");

        KeyPair selectedSignature = signatures.FirstOrDefault(x => x.Id == signatureId.Value);

        return selectedSignature == null
            ? throw new InvalidSignatureIdException(signatureId.Value.ToString())
            : selectedSignature;
    }
}