using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;

internal class InitializeMainUseCase : IQuery<InitializeMainRequest, InitializeMainResponse>
{
    private readonly ISignatureKeyRepository _signatureKeyRepository;

    public InitializeMainUseCase(ISignatureKeyRepository signatureKeyRepository)
    {
        _signatureKeyRepository = signatureKeyRepository ?? throw new ArgumentNullException(nameof(signatureKeyRepository));
    }

    public Task<InitializeMainResponse> Query(InitializeMainRequest criteria)
    {
        InitializeMainResponse response = new()
        {
            SignatureKeys = LoadSignatureKeys()
        };

        return Task.FromResult(response);
    }

    private List<SignatureKeyDto> LoadSignatureKeys()
    {
        return _signatureKeyRepository.GetAll()
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
