using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;

namespace DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;

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
            SelectedSignatureKeyId = applicationState.SelectedSignatureKeyId
        };

        return Task.FromResult(response);
    }

    private List<SignatureKeyDto> LoadSignatureKeys()
    {
        return signatureKeyRepository.GetAll()
            .Select(ToDto)
            .ToList();
    }

    private SignatureKeyDto ToDto(SignatureKey key)
    {
        return new SignatureKeyDto
        {
            Id = key.Id,
            CreatedDate = key.CreatedDate
        };
    }
}
