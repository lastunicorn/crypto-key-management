using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;

internal class PresentMainUseCase : IQuery<PresentMainRequest, PresentMainResponse>
{
    private readonly ISignatureKeyRepository signatureKeyRepository;
    private readonly IApplicationState applicationState;

    public PresentMainUseCase(ISignatureKeyRepository signatureKeyRepository, IApplicationState applicationState)
    {
        this.signatureKeyRepository = signatureKeyRepository ?? throw new ArgumentNullException(nameof(signatureKeyRepository));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public Task<PresentMainResponse> Query(PresentMainRequest criteria)
    {
        PresentMainResponse response = new()
        {
            SignatureKeys = LoadSignatureKeys(),
            SelectedSignatureKeyId = applicationState.CurrentSignatureKey?.Id
        };

        return Task.FromResult(response);
    }

    private List<KeyPairDto> LoadSignatureKeys()
    {
        return signatureKeyRepository.GetAll()
            .Select(SignatureKeyExtensions.ToDto)
            .ToList();
    }
}
