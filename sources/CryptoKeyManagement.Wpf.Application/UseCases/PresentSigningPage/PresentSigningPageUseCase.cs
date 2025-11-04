using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

internal class PresentSigningPageUseCase : IQuery<PresentSigningPageRequest, PresentSigningPageResponse>
{
    private readonly ICryptoKeyRepository cryptoKeyRepository;
    private readonly IApplicationState applicationState;

    public PresentSigningPageUseCase(ICryptoKeyRepository cryptoKeyRepository, IApplicationState applicationState)
    {
        this.cryptoKeyRepository = cryptoKeyRepository ?? throw new ArgumentNullException(nameof(cryptoKeyRepository));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public Task<PresentSigningPageResponse> Query(PresentSigningPageRequest criteria)
    {
        PresentSigningPageResponse response = new()
        {
            KeyPairs = LoadSignatureKeys(),
            SelectedKeyPairId = applicationState.CurrentSignatureKey?.Id
        };

        return Task.FromResult(response);
    }

    private List<KeyPairDto> LoadSignatureKeys()
    {
        return cryptoKeyRepository.GetAll()
            .Select(KeyPairExtensions.ToDto)
            .ToList();
    }
}
