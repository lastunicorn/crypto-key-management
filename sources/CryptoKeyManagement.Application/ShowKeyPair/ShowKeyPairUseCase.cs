using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;

namespace DustInTheWind.CryptoKeyManagement.Application.ShowKeyPair;

internal class ShowKeyPairUseCase : IQuery<ShowKeyPairRequest, ShowKeyPairResponse>
{
    private readonly ICryptoKeyRepository signatureRepository;

    public ShowKeyPairUseCase(ICryptoKeyRepository signatureRepository)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
    }

    public Task<ShowKeyPairResponse> Query(ShowKeyPairRequest criteria)
    {
        List<KeyPair> signatures = signatureRepository.GetAll()
            .ToList();

        IEnumerable<KeyPairDetails> signatureDetails = signatures
            .Select(x => ToSignatureDetails(x));

        ShowKeyPairResponse response = new()
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