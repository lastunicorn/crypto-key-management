using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Ports.CryptographyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.UserAccess;

namespace DustInTheWind.CryptoKeyManagement.Application.SignData;

internal class SignDataUseCase : IQuery<SignDataCriteria, SignDataResponse>
{
    private readonly ICryptoKeyRepository cryptoKeyRepository;
    private readonly IUserConsole userConsole;
    private readonly ICryptographyService cryptographyService;

    public SignDataUseCase(ICryptoKeyRepository cryptoKeyRepository, IUserConsole userConsole, ICryptographyService cryptographyService)
    {
        this.cryptoKeyRepository = cryptoKeyRepository ?? throw new ArgumentNullException(nameof(cryptoKeyRepository));
        this.userConsole = userConsole ?? throw new ArgumentNullException(nameof(userConsole));
        this.cryptographyService = cryptographyService ?? throw new ArgumentNullException(nameof(cryptographyService));
    }

    public Task<SignDataResponse> Query(SignDataCriteria criteria)
    {
        List<KeyPair> keys = GetAllKeys();
        DisplayKeys(keys);

        KeyPair selectedKeyPair = AskForKeyToUse(keys);

        string dataToSign = userConsole.AskForDataToSign();
        byte[] signature = cryptographyService.Sign(selectedKeyPair, dataToSign);

        SignDataResponse result = new()
        {
            Signature = signature
        };

        return Task.FromResult(result);
    }

    private List<KeyPair> GetAllKeys()
    {
        List<KeyPair> keyPairs = cryptoKeyRepository.GetAll()
            .ToList();

        if (keyPairs.Count == 0)
            throw new NoKeysException();

        return keyPairs;
    }

    private void DisplayKeys(List<KeyPair> keyPairs)
    {
        IEnumerable<KeyPairSummary> keyPairSummaries = keyPairs
            .Select(x => new KeyPairSummary
            {
                Id = x.Id,
                PrivateKey = x.PrivateKey,
                PublicKey = x.PublicKey,
                CreatedDate = x.CreatedDate
            });

        userConsole.DisplayKeyPairs(keyPairSummaries);
    }

    private KeyPair AskForKeyToUse(List<KeyPair> keyPairs)
    {
        Guid? keyPairId = userConsole.AskKeyPairId();

        if (!keyPairId.HasValue)
            throw new InvalidKeyPairIdException("Invalid GUID format");

        KeyPair selectedKeyPair = keyPairs.FirstOrDefault(x => x.Id == keyPairId.Value);

        return selectedKeyPair == null
            ? throw new InvalidKeyPairIdException(keyPairId.Value.ToString())
            : selectedKeyPair;
    }
}