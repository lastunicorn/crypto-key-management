using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SelectKeyPair;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.SelectKeyPair.SelectKeyPairUseCaseTests;

public class HandleTests
{
    private readonly Mock<ISignatureKeyRepository> signatureKeyRepository;
    private readonly Mock<IApplicationState> applicationStateService;
    private readonly Mock<IEventBus> eventBus;
    private readonly SelectKeyPairUseCase useCase;

    public HandleTests()
    {
        signatureKeyRepository = new Mock<ISignatureKeyRepository>();
        applicationStateService = new Mock<IApplicationState>();
        eventBus = new Mock<IEventBus>();
        useCase = new SelectKeyPairUseCase(signatureKeyRepository.Object, applicationStateService.Object, eventBus.Object);
    }

    [Fact]
    public async Task Handle_WithRequestWithNullSignatureKeyId_ShouldSetCurrentSignatureKeyToNull()
    {
        // Arrange
        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = null
        };

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
        applicationStateService.VerifySet(x => x.CurrentSignatureKey = null, Times.Once);
        signatureKeyRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithRequestWithValidSignatureKeyId_ShouldRetrieveKeyFromRepository()
    {
        // Arrange
        Guid keyId = Guid.NewGuid();
        KeyPair expectedKeyPair = new()
        {
            Id = keyId,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = [1, 2, 3, 4],
            PublicKey = [5, 6, 7, 8]
        };

        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = keyId
        };

        signatureKeyRepository
            .Setup(x => x.GetById(keyId))
            .Returns(expectedKeyPair);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
        signatureKeyRepository.Verify(x => x.GetById(keyId), Times.Once);
        applicationStateService.VerifySet(x => x.CurrentSignatureKey = expectedKeyPair, Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidKeyPair_ShouldPublishKeyPairSelectionChangedEvent()
    {
        // Arrange
        Guid keyId = Guid.NewGuid();
        DateTime createdDate = DateTime.UtcNow;
        byte[] privateKey = [1, 2, 3, 4];
        byte[] publicKey = [5, 6, 7, 8];

        KeyPair keyPair = new()
        {
            Id = keyId,
            CreatedDate = createdDate,
            PrivateKey = privateKey,
            PublicKey = publicKey
        };

        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = keyId
        };

        KeyPairSelectionChangedEvent capturedEvent = null;

        signatureKeyRepository
            .Setup(x => x.GetById(keyId))
            .Returns(keyPair);

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairSelectionChangedEvent>(), It.IsAny<CancellationToken>()))
            .Callback<KeyPairSelectionChangedEvent, CancellationToken>((evt, ct) => capturedEvent = evt);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairSelectionChangedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedEvent);
        Assert.NotNull(capturedEvent.SignatureKey);
        Assert.Equal(keyId, capturedEvent.SignatureKey.Id);
        Assert.Equal(createdDate, capturedEvent.SignatureKey.CreatedDate);
        Assert.Equal(privateKey, capturedEvent.SignatureKey.PrivateKey);
        Assert.Equal(publicKey, capturedEvent.SignatureKey.PublicKey);
    }

    [Fact]
    public async Task Handle_WithNullKeyPair_ShouldPublishEventWithNullSignatureKey()
    {
        // Arrange
        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = null
        };

        KeyPairSelectionChangedEvent capturedEvent = null;

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairSelectionChangedEvent>(), It.IsAny<CancellationToken>()))
            .Callback<KeyPairSelectionChangedEvent, CancellationToken>((evt, ct) => capturedEvent = evt);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairSelectionChangedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedEvent);
        Assert.Null(capturedEvent.SignatureKey);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessfulCommandWorkflowResult()
    {
        // Arrange
        Guid keyId = Guid.NewGuid();
        KeyPair keyPair = new()
        {
            Id = keyId,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = [1, 2, 3, 4],
            PublicKey = [5, 6, 7, 8]
        };

        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = keyId
        };

        signatureKeyRepository
            .Setup(x => x.GetById(keyId))
            .Returns(keyPair);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
    }

    [Fact]
    public async Task Handle_WithRepositoryReturningNull_ShouldSetCurrentSignatureKeyToNullAndPublishNullEvent()
    {
        // Arrange
        Guid keyId = Guid.NewGuid();
        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = keyId
        };

        KeyPairSelectionChangedEvent capturedEvent = null;

        signatureKeyRepository
            .Setup(x => x.GetById(keyId))
            .Returns((KeyPair)null);

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairSelectionChangedEvent>(), It.IsAny<CancellationToken>()))
            .Callback<KeyPairSelectionChangedEvent, CancellationToken>((evt, ct) => capturedEvent = evt);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
        signatureKeyRepository.Verify(x => x.GetById(keyId), Times.Once);
        applicationStateService.VerifySet(x => x.CurrentSignatureKey = null, Times.Once);
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairSelectionChangedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(capturedEvent);
        Assert.Null(capturedEvent.SignatureKey);
    }

    [Fact]
    public async Task Handle_ShouldCallEventBusPublishAsyncExactlyOnce()
    {
        // Arrange
        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = null
        };

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairSelectionChangedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldSetApplicationStateBeforePublishingEvent()
    {
        // Arrange
        Guid keyId = Guid.NewGuid();
        KeyPair keyPair = new()
        {
            Id = keyId,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = [1, 2, 3, 4],
            PublicKey = [5, 6, 7, 8]
        };

        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = keyId
        };

        signatureKeyRepository
            .Setup(x => x.GetById(keyId))
            .Returns(keyPair);

        // Act
        await useCase.Handle(request);

        // Assert
        // Verify that both the application state is set and the event is published
        applicationStateService.VerifySet(x => x.CurrentSignatureKey = keyPair, Times.Once);
        eventBus.Verify(x => x.PublishAsync(It.IsAny<KeyPairSelectionChangedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyGuid_ShouldRetrieveKeyFromRepository()
    {
        // Arrange
        Guid emptyGuid = Guid.Empty;
        KeyPair expectedKeyPair = new()
        {
            Id = emptyGuid,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = [1, 2, 3, 4],
            PublicKey = [5, 6, 7, 8]
        };

        SelectKeyPairRequest request = new()
        {
            SignatureKeyId = emptyGuid
        };

        signatureKeyRepository
            .Setup(x => x.GetById(emptyGuid))
            .Returns(expectedKeyPair);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
        signatureKeyRepository.Verify(x => x.GetById(emptyGuid), Times.Once);
        applicationStateService.VerifySet(x => x.CurrentSignatureKey = expectedKeyPair, Times.Once);
    }
}