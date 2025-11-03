using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.RefreshKeyPairs;

/// <summary>
/// Use case for refreshing the signature keys list.
/// </summary>
internal class RefreshKeyPairsUseCase : ICommandHandler<RefreshKeyPairsRequest>
{
    private readonly ISignatureKeyRepository signatureKeyRepository;
    private readonly IEventBus eventBus;

    public RefreshKeyPairsUseCase(ISignatureKeyRepository signatureKeyRepository, IEventBus eventBus)
    {
        this.signatureKeyRepository = signatureKeyRepository ?? throw new ArgumentNullException(nameof(signatureKeyRepository));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<ICommandWorkflowResult> Handle(RefreshKeyPairsRequest command)
    {
        List<KeyPairDto> signatureKeys = LoadSignatureKeys();

        await PublishKeyPairsRefreshEvent(signatureKeys);

        return CommandWorkflowResult.Ok();
    }

    private List<KeyPairDto> LoadSignatureKeys()
    {
        return signatureKeyRepository.GetAll()
            .Select(SignatureKeyExtensions.ToDto)
            .ToList();
    }

    private async Task PublishKeyPairsRefreshEvent(List<KeyPairDto> signatureKeys)
    {
        KeyPairsRefreshEvent @event = new()
        {
            SignatureKeys = signatureKeys
        };

        await eventBus.PublishAsync(@event);
    }
}