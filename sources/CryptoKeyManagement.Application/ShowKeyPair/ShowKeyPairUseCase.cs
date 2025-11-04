using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;

namespace DustInTheWind.CryptoKeyManagement.Application.ShowKeyPair;

internal class ShowKeyPairUseCase : IQuery<ShowKeyPairRequest, ShowKeyPairResponse>
{
    private readonly ICryptoKeyRepository cryptoKeyRepository;

    public ShowKeyPairUseCase(ICryptoKeyRepository cryptoKeyRepository)
    {
        this.cryptoKeyRepository = cryptoKeyRepository ?? throw new ArgumentNullException(nameof(cryptoKeyRepository));
    }

    public Task<ShowKeyPairResponse> Query(ShowKeyPairRequest criteria)
    {
        List<KeyPair> keyPairs = cryptoKeyRepository.GetAll()
            .ToList();

        IEnumerable<KeyPairDetails> keyPairDetails = keyPairs
            .Select(x => ToKeyPairDetails(x));

        ShowKeyPairResponse response = new()
        {
            KeyPairs = keyPairDetails
        };

        return Task.FromResult(response);
    }

    private static KeyPairDetails ToKeyPairDetails(KeyPair x)
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