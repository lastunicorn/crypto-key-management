using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.RefreshKeyPairs;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.RefreshKeyPairs.RefreshKeyPairsUseCaseTests;

public class HandleTests
{
    private readonly Mock<ISignatureKeyRepository> signatureKeyRepository;
    private readonly Mock<IEventBus> eventBus;
    private readonly RefreshKeyPairsUseCase useCase;

    public HandleTests()
    {
        signatureKeyRepository = new Mock<ISignatureKeyRepository>();
        eventBus = new Mock<IEventBus>();
        useCase = new RefreshKeyPairsUseCase(signatureKeyRepository.Object, eventBus.Object);
    }

    [Fact]
    public async Task Handle_WithNullRequest_ShouldNotThrow()
    {
        // Arrange
        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        // Act & Assert
        ICommandWorkflowResult result = await useCase.Handle(null);

        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnSuccessfulResult()
    {
        // Arrange
        RefreshKeyPairsRequest request = new();
        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
    }

    [Fact]
    public async Task Handle_ShouldCallSignatureKeyRepositoryGetAll()
    {
        // Arrange
        RefreshKeyPairsRequest request = new();
        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        // Act
        await useCase.Handle(request);

        // Assert
        signatureKeyRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyKeyPairsList_ShouldPublishEventWithEmptyList()
    {
        // Arrange
        RefreshKeyPairsRequest request = new();
        List<KeyPair> emptyKeyPairs = [];
        KeyPairsRefreshEvent capturedEvent = null;

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(emptyKeyPairs);

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()))
            .Callback<KeyPairsRefreshEvent, CancellationToken>((evt, ct) => capturedEvent = evt);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedEvent);
        Assert.Empty(capturedEvent.SignatureKeys);
    }

    [Fact]
    public async Task Handle_WithSingleKeyPair_ShouldPublishEventWithConvertedKeyPair()
    {
        // Arrange
        RefreshKeyPairsRequest request = new();
        Guid keyPairId = Guid.NewGuid();
        DateTime createdDate = DateTime.UtcNow;
        byte[] privateKey = [1, 2, 3, 4];
        byte[] publicKey = [5, 6, 7, 8];

        KeyPair keyPair = new()
        {
            Id = keyPairId,
            CreatedDate = createdDate,
            PrivateKey = privateKey,
            PublicKey = publicKey
        };

        List<KeyPair> keyPairs = [keyPair];
        KeyPairsRefreshEvent capturedEvent = null;

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(keyPairs);

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()))
            .Callback<KeyPairsRefreshEvent, CancellationToken>((evt, ct) => capturedEvent = evt);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedEvent);
        Assert.Single(capturedEvent.SignatureKeys);

        KeyPairDto convertedKeyPair = capturedEvent.SignatureKeys.First();
        Assert.Equal(keyPairId, convertedKeyPair.Id);
        Assert.Equal(createdDate, convertedKeyPair.CreatedDate);
        Assert.Equal(privateKey, convertedKeyPair.PrivateKey);
        Assert.Equal(publicKey, convertedKeyPair.PublicKey);
    }

    [Fact]
    public async Task Handle_WithMultipleKeyPairs_ShouldPublishEventWithAllConvertedKeyPairs()
    {
        // Arrange
        RefreshKeyPairsRequest request = new();

        KeyPair keyPair1 = new()
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            PrivateKey = [1, 2, 3],
            PublicKey = [4, 5, 6]
        };

        KeyPair keyPair2 = new()
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            PrivateKey = [7, 8, 9],
            PublicKey = [10, 11, 12]
        };

        List<KeyPair> keyPairs = [keyPair1, keyPair2];
        KeyPairsRefreshEvent capturedEvent = null;

        _ = signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(keyPairs);

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()))
            .Callback<KeyPairsRefreshEvent, CancellationToken>((evt, ct) => capturedEvent = evt);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedEvent);
        Assert.Equal(2, capturedEvent.SignatureKeys.Count);

        // Verify first key pair conversion
        KeyPairDto convertedKeyPair1 = capturedEvent.SignatureKeys.First(x => x.Id == keyPair1.Id);
        Assert.Equal(keyPair1.CreatedDate, convertedKeyPair1.CreatedDate);
        Assert.Equal(keyPair1.PrivateKey, convertedKeyPair1.PrivateKey);
        Assert.Equal(keyPair1.PublicKey, convertedKeyPair1.PublicKey);

        // Verify second key pair conversion
        KeyPairDto convertedKeyPair2 = capturedEvent.SignatureKeys.First(x => x.Id == keyPair2.Id);
        Assert.Equal(keyPair2.CreatedDate, convertedKeyPair2.CreatedDate);
        Assert.Equal(keyPair2.PrivateKey, convertedKeyPair2.PrivateKey);
        Assert.Equal(keyPair2.PublicKey, convertedKeyPair2.PublicKey);
    }

    [Fact]
    public async Task Handle_ShouldPublishEventOnlyOnce()
    {
        // Arrange
        RefreshKeyPairsRequest request = new();
        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        RefreshKeyPairsRequest request = new();
        InvalidOperationException expectedException = new("Repository error");

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Throws(expectedException);

        // Act & Assert
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            return useCase.Handle(request);
        });

        Assert.Equal(expectedException.Message, exception.Message);
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenEventBusThrowsException_ShouldPropagateException()
    {
        // Arrange
        RefreshKeyPairsRequest request = new();
        InvalidOperationException expectedException = new("EventBus error");

        signatureKeyRepository
            .Setup(x => x.GetAll())
            .Returns(new List<KeyPair>());

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairsRefreshEvent>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            return useCase.Handle(request);
        });

        Assert.Equal(expectedException.Message, exception.Message);
        signatureKeyRepository.Verify(x => x.GetAll(), Times.Once);
    }
}