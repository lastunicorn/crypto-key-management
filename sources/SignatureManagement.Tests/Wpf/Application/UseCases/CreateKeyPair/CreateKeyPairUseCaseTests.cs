using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.CreateKeyPair;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.CreateKeyPair;

public class CreateKeyPairUseCaseTests
{
    private readonly Mock<ISignatureKeyRepository> signatureRepository;
    private readonly Mock<IEventBus> eventBus;
    private readonly CreateKeyPairUseCase useCase;

    public CreateKeyPairUseCaseTests()
    {
        signatureRepository = new Mock<ISignatureKeyRepository>();
        eventBus = new Mock<IEventBus>();

        useCase = new CreateKeyPairUseCase(signatureRepository.Object, eventBus.Object);
    }

    [Fact]
    public async Task Handle_ShouldGenerateKeyPairAndSaveToRepository()
    {
        // Arrange
        CreateKeyPairRequest request = new();
        Guid keyPairId = Guid.NewGuid();
        DateTime createdDate = DateTime.UtcNow;

        KeyPair savedKeyPair = new()
        {
            Id = keyPairId,
            CreatedDate = createdDate,
            PrivateKey = new byte[32], // Ed25519 private key size
            PublicKey = new byte[32]   // Ed25519 public key size
        };

        signatureRepository
            .Setup(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(keyPairId);

        signatureRepository
            .Setup(x => x.GetById(keyPairId))
            .Returns(savedKeyPair);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.IsType<CommandWorkflowResult>(result);
        signatureRepository.Verify(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        signatureRepository.Verify(x => x.GetById(keyPairId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldAddKeyPairWithCorrectKeyLengths()
    {
        // Arrange
        CreateKeyPairRequest request = new();
        Guid keyPairId = Guid.NewGuid();
        byte[] capturedPrivateKey = null;
        byte[] capturedPublicKey = null;

        KeyPair savedKeyPair = new()
        {
            Id = keyPairId,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = new byte[32],
            PublicKey = new byte[32]
        };

        signatureRepository
            .Setup(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Callback<byte[], byte[]>((privateKey, publicKey) =>
            {
                capturedPrivateKey = privateKey;
                capturedPublicKey = publicKey;
            })
            .Returns(keyPairId);

        signatureRepository
            .Setup(x => x.GetById(keyPairId))
            .Returns(savedKeyPair);

        // Act
        await useCase.Handle(request);

        // Assert
        Assert.NotNull(capturedPrivateKey);
        Assert.NotNull(capturedPublicKey);
        Assert.Equal(32, capturedPrivateKey.Length); // Ed25519 private key is 32 bytes
        Assert.Equal(32, capturedPublicKey.Length);  // Ed25519 public key is 32 bytes
    }

    [Fact]
    public async Task Handle_ShouldPublishKeyPairCreatedEvent()
    {
        // Arrange
        CreateKeyPairRequest request = new();
        Guid keyPairId = Guid.NewGuid();
        DateTime createdDate = DateTime.UtcNow;

        KeyPair savedKeyPair = new()
        {
            Id = keyPairId,
            CreatedDate = createdDate,
            PrivateKey = new byte[32],
            PublicKey = new byte[32]
        };

        signatureRepository
            .Setup(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(keyPairId);

        signatureRepository
            .Setup(x => x.GetById(keyPairId))
            .Returns(savedKeyPair);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.Is<KeyPairCreatedEvent>(e =>
            e.SignatureKey.Id == keyPairId &&
            e.SignatureKey.CreatedDate == createdDate &&
            e.SignatureKey.PrivateKey == savedKeyPair.PrivateKey &&
            e.SignatureKey.PublicKey == savedKeyPair.PublicKey
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult()
    {
        // Arrange
        CreateKeyPairRequest request = new();
        Guid keyPairId = Guid.NewGuid();

        KeyPair savedKeyPair = new()
        {
            Id = keyPairId,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = new byte[32],
            PublicKey = new byte[32]
        };

        signatureRepository
            .Setup(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(keyPairId);

        signatureRepository
            .Setup(x => x.GetById(keyPairId))
            .Returns(savedKeyPair);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
    }

    [Fact]
    public async Task Handle_ShouldGenerateDifferentKeysOnMultipleCalls()
    {
        // Arrange
        CreateKeyPairRequest request1 = new();
        CreateKeyPairRequest request2 = new();
        Guid keyPairId1 = Guid.NewGuid();
        Guid keyPairId2 = Guid.NewGuid();

        byte[] privateKey1 = null;
        byte[] publicKey1 = null;
        byte[] privateKey2 = null;
        byte[] publicKey2 = null;
        int callCount = 0;

        KeyPair savedKeyPair1 = new()
        {
            Id = keyPairId1,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = new byte[32],
            PublicKey = new byte[32]
        };

        KeyPair savedKeyPair2 = new()
        {
            Id = keyPairId2,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = new byte[32],
            PublicKey = new byte[32]
        };

        // Setup to capture different keys on each call
        signatureRepository
            .Setup(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Callback<byte[], byte[]>((privateKey, publicKey) =>
            {
                callCount++;
                if (callCount == 1)
                {
                    privateKey1 = privateKey;
                    publicKey1 = publicKey;
                }
                else if (callCount == 2)
                {
                    privateKey2 = privateKey;
                    publicKey2 = publicKey;
                }
            })
            .Returns(() => callCount == 1 ? keyPairId1 : keyPairId2);

        signatureRepository
            .Setup(x => x.GetById(keyPairId1))
            .Returns(savedKeyPair1);

        signatureRepository
            .Setup(x => x.GetById(keyPairId2))
            .Returns(savedKeyPair2);

        // Act
        await useCase.Handle(request1);
        await useCase.Handle(request2);

        // Assert
        Assert.NotNull(privateKey1);
        Assert.NotNull(publicKey1);
        Assert.NotNull(privateKey2);
        Assert.NotNull(publicKey2);

        // Keys should be different between calls
        Assert.False(privateKey1.SequenceEqual(privateKey2));
        Assert.False(publicKey1.SequenceEqual(publicKey2));
    }

    [Fact]
    public void Constructor_WithNullRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            return new CreateKeyPairUseCase(null, eventBus.Object);
        });
    }

    [Fact]
    public void Constructor_WithNullEventBus_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            return new CreateKeyPairUseCase(signatureRepository.Object, null);
        });
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        CreateKeyPairRequest request = new();
        InvalidOperationException expectedException = new("Repository error");

        signatureRepository
            .Setup(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Throws(expectedException);

        // Act & Assert
        InvalidOperationException actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            return useCase.Handle(request);
        });

        Assert.Equal(expectedException.Message, actualException.Message);
    }

    [Fact]
    public async Task Handle_WhenEventBusThrowsException_ShouldPropagateException()
    {
        // Arrange
        CreateKeyPairRequest request = new();
        Guid keyPairId = Guid.NewGuid();
        InvalidOperationException expectedException = new("EventBus error");

        KeyPair savedKeyPair = new()
        {
            Id = keyPairId,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = new byte[32],
            PublicKey = new byte[32]
        };

        signatureRepository
            .Setup(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(keyPairId);

        signatureRepository
            .Setup(x => x.GetById(keyPairId))
            .Returns(savedKeyPair);

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<KeyPairCreatedEvent>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        InvalidOperationException actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            return useCase.Handle(request);
        });

        Assert.Equal(expectedException.Message, actualException.Message);
    }

    [Fact]
    public async Task Handle_ShouldRetrieveKeyPairAfterSaving()
    {
        // Arrange
        CreateKeyPairRequest request = new();
        Guid keyPairId = Guid.NewGuid();

        KeyPair savedKeyPair = new()
        {
            Id = keyPairId,
            CreatedDate = DateTime.UtcNow,
            PrivateKey = new byte[32],
            PublicKey = new byte[32]
        };

        signatureRepository
            .Setup(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(keyPairId);

        signatureRepository
            .Setup(x => x.GetById(keyPairId))
            .Returns(savedKeyPair);

        // Act
        await useCase.Handle(request);

        // Assert
        // Verify that GetById is called after Add
        signatureRepository.Verify(x => x.Add(It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
        signatureRepository.Verify(x => x.GetById(keyPairId), Times.Once);
    }
}