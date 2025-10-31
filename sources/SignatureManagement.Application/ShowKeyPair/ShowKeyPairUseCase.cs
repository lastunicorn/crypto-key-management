using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace DustInTheWind.SignatureManagement.Application.ShowKeyPair;

internal class ShowKeyPairUseCase : IQuery<ShowKeyPairRequest, ShowKeyPairResponse>
{
    private readonly ISignatureKeyRepository signatureRepository;

    public ShowKeyPairUseCase(ISignatureKeyRepository signatureRepository)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
    }

    public Task<ShowKeyPairResponse> Query(ShowKeyPairRequest criteria)
    {
        List<KeyPair> signatures = signatureRepository.GetAll()
            .ToList();

        IEnumerable<KeyPairDetails> signatureDetails = signatures
            .Select(x => ToSignatureDetails(x));

        var response = new ShowKeyPairResponse
        {
            Signatures = signatureDetails
        };

        return Task.FromResult(response);
    }

    private static KeyPairDetails ToSignatureDetails(KeyPair x)
    {
        return new KeyPairDetails
        {
            Id = x.Id,
            PrivateKeyValue = Convert.ToBase64String(x.PrivateKey),
            PublicKeyValue = Convert.ToBase64String(x.PublicKey),
            Created = x.CreatedDate
        };
    }
}