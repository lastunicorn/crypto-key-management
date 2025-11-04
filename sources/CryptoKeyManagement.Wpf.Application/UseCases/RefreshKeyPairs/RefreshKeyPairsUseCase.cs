using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.RefreshKeyPairs;

/// <summary>
/// Use case for refreshing the signature keys list.
/// </summary>
internal class RefreshKeyPairsUseCase : ICommandHandler<RefreshKeyPairsRequest>
{
    private readonly ICryptoKeyRepository signatureKeyRepository;
    private readonly IEventBus eventBus;

    public RefreshKeyPairsUseCase(ICryptoKeyRepository signatureKeyRepository, IEventBus eventBus)
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
            .Select(KeyPairExtensions.ToDto)
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