using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

internal class PresentSigningPageUseCase : IQuery<PresentSigningPageRequest, PresentSigningPageResponse>
{
    private readonly ISignatureKeyRepository signatureKeyRepository;
    private readonly IApplicationState applicationState;

    public PresentSigningPageUseCase(ISignatureKeyRepository signatureKeyRepository, IApplicationState applicationState)
    {
        this.signatureKeyRepository = signatureKeyRepository ?? throw new ArgumentNullException(nameof(signatureKeyRepository));
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
        return signatureKeyRepository.GetAll()
            .Select(KeyPairExtensions.ToDto)
            .ToList();
    }
}
