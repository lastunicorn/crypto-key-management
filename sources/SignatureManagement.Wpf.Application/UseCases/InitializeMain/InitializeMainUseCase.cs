using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

internal class InitializeMainUseCase : IQuery<InitializeMainRequest, InitializeMainResponse>
{
    private readonly ISignatureKeyRepository signatureKeyRepository;
    private readonly IApplicationState applicationState;

    public InitializeMainUseCase(ISignatureKeyRepository signatureKeyRepository, IApplicationState applicationState)
    {
        this.signatureKeyRepository = signatureKeyRepository ?? throw new ArgumentNullException(nameof(signatureKeyRepository));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public Task<InitializeMainResponse> Query(InitializeMainRequest criteria)
    {

        InitializeMainResponse response = new()
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
