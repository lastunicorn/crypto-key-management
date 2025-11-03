using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.DeleteKeyPair;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.DeleteKeyPair;

public class DeleteKeyPairUseCaseTests
{
    private readonly Mock<ISignatureKeyRepository> signatureKeyRepository;
    private readonly Mock<IApplicationState> applicationState;
    private readonly Mock<IEventBus> eventBus;
    private readonly DeleteKeyPairUseCase useCase;

    public DeleteKeyPairUseCaseTests()
    {
        signatureKeyRepository = new Mock<ISignatureKeyRepository>();
        applicationState = new Mock<IApplicationState>();
        eventBus = new Mock<IEventBus>();

        useCase = new DeleteKeyPairUseCase(signatureKeyRepository.Object, applicationState.Object, eventBus.Object);
    }

    [Fact]
    public void Constructor_WithNullSignatureKeyRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            return new DeleteKeyPairUseCase(null, applicationState.Object, eventBus.Object);
        });

        Assert.Equal("signatureKeyRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullApplicationState_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            return new DeleteKeyPairUseCase(signatureKeyRepository.Object, null, eventBus.Object);
        });

        Assert.Equal("applicationState", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullEventBus_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            return new DeleteKeyPairUseCase(signatureKeyRepository.Object, applicationState.Object, null);
        });

        Assert.Equal("eventBus", exception.ParamName);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldDeleteKeyPairFromRepository()
    {
        // Arrange
        Guid keyPairId = Guid.NewGuid();
        DeleteKeyPairRequest request = new()
        {
            KeyPairId = keyPairId
        };

        applicationState.SetupProperty(x => x.CurrentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        signatureKeyRepository.Verify(x => x.Delete(keyPairId), Times.Once);
        Assert.IsType<CommandWorkflowResult>(result);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldPublishKeyPairDeletedEvent()
    {
        // Arrange
        Guid keyPairId = Guid.NewGuid();
        DeleteKeyPairRequest request = new()
        {
            KeyPairId = keyPairId
        };

        applicationState.SetupProperty(x => x.CurrentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(
            It.Is<KeyPairDeletedEvent>(e => e.KeyPairId == keyPairId),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldReturnSuccessResult()
    {
        // Arrange
        Guid keyPairId = Guid.NewGuid();
        DeleteKeyPairRequest request = new()
        {
            KeyPairId = keyPairId
        };

        applicationState.SetupProperty(x => x.CurrentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
    }

    [Fact]
    public async Task Handle_WhenKeyPairIsNotCurrentlySelected_ShouldNotClearApplicationState()
    {
        // Arrange
        Guid keyPairIdToDelete = Guid.NewGuid();
        Guid currentSelectedKeyPairId = Guid.NewGuid();

        KeyPair currentSignatureKey = new() { Id = currentSelectedKeyPairId };
        string currentMessage = "test message";
        byte[] currentSignature = [1, 2, 3, 4];

        DeleteKeyPairRequest request = new()
        {
            KeyPairId = keyPairIdToDelete
        };

        applicationState.SetupProperty(x => x.CurrentSignatureKey, currentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage, currentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature, currentSignature);

        // Act
        await useCase.Handle(request);

        // Assert
        Assert.Equal(currentSignatureKey, applicationState.Object.CurrentSignatureKey);
        Assert.Equal(currentMessage, applicationState.Object.CurrentMessage);
        Assert.Equal(currentSignature, applicationState.Object.CurrentSignature);
    }

    [Fact]
    public async Task Handle_WhenKeyPairIsCurrentlySelected_ShouldClearApplicationState()
    {
        // Arrange
        Guid keyPairId = Guid.NewGuid();
        KeyPair currentSignatureKey = new() { Id = keyPairId };
        string currentMessage = "test message";
        byte[] currentSignature = [1, 2, 3, 4];

        DeleteKeyPairRequest request = new() { KeyPairId = keyPairId };

        applicationState.SetupProperty(x => x.CurrentSignatureKey, currentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage, currentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature, currentSignature);

        // Act
        await useCase.Handle(request);

        // Assert
        Assert.Null(applicationState.Object.CurrentSignatureKey);
        Assert.Null(applicationState.Object.CurrentMessage);
        Assert.Null(applicationState.Object.CurrentSignature);
    }

    [Fact]
    public async Task Handle_WhenCurrentSignatureKeyIsNull_ShouldNotThrowException()
    {
        // Arrange
        Guid keyPairId = Guid.NewGuid();
        DeleteKeyPairRequest request = new()
        {
            KeyPairId = keyPairId
        };

        applicationState.SetupProperty(x => x.CurrentSignatureKey, (KeyPair)null);
        applicationState.SetupProperty(x => x.CurrentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature);

        // Act & Assert
        ICommandWorkflowResult result = await useCase.Handle(request);

        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
    }

    [Fact]
    public async Task Handle_ShouldExecuteOperationsInCorrectOrder()
    {
        // Arrange
        Guid keyPairId = Guid.NewGuid();
        DeleteKeyPairRequest request = new()
        {
            KeyPairId = keyPairId
        };
        List<string> operationOrder = [];

        applicationState.SetupProperty(x => x.CurrentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature);

        signatureKeyRepository
            .Setup(x => x.Delete(It.IsAny<Guid>()))
        .Callback(() => operationOrder.Add("Delete"));

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairDeletedEvent>(), It.IsAny<CancellationToken>()))
            .Callback(() => operationOrder.Add("PublishEvent"))
            .Returns(Task.CompletedTask);

        // Act
        await useCase.Handle(request);

        // Assert
        Assert.Equal(2, operationOrder.Count);
        Assert.Equal("Delete", operationOrder[0]);
        Assert.Equal("PublishEvent", operationOrder[1]);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagate()
    {
        // Arrange
        Guid keyPairId = Guid.NewGuid();
        DeleteKeyPairRequest request = new()
        {
            KeyPairId = keyPairId
        };
        Exception expectedException = new InvalidOperationException("Repository error");

        applicationState.SetupProperty(x => x.CurrentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature);

        signatureKeyRepository
            .Setup(x => x.Delete(It.IsAny<Guid>()))
            .Throws(expectedException);

        // Act & Assert
        Exception actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Handle(request));
        Assert.Equal(expectedException.Message, actualException.Message);
    }

    [Fact]
    public async Task Handle_WhenEventBusThrowsException_ShouldPropagate()
    {
        // Arrange
        Guid keyPairId = Guid.NewGuid();
        DeleteKeyPairRequest request = new() { KeyPairId = keyPairId };
        Exception expectedException = new InvalidOperationException("EventBus error");

        applicationState.SetupProperty(x => x.CurrentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature);

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairDeletedEvent>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        Exception actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Handle(request));
        Assert.Equal(expectedException.Message, actualException.Message);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public async Task Handle_WithDifferentGuidValues_ShouldDeleteCorrectKey(string guidString)
    {
        // Arrange
        Guid keyPairId = Guid.Parse(guidString);
        DeleteKeyPairRequest request = new() { KeyPairId = keyPairId };

        applicationState.SetupProperty(x => x.CurrentSignatureKey);
        applicationState.SetupProperty(x => x.CurrentMessage);
        applicationState.SetupProperty(x => x.CurrentSignature);

        // Act
        await useCase.Handle(request);

        // Assert
        signatureKeyRepository.Verify(x => x.Delete(keyPairId), Times.Once);
        eventBus.Verify(x => x.PublishAsync(
            It.Is<KeyPairDeletedEvent>(e => e.KeyPairId == keyPairId),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}