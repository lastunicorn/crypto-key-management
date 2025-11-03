using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptographyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SignMessage;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.SignMessage.SignMessageUseCaseTests;

public class HandleTests
{
    private readonly Mock<IApplicationState> applicationState;
    private readonly Mock<IEventBus> eventBus;
    private readonly Mock<ICryptographyService> cryptographyService;
    private readonly SignMessageUseCase useCase;

    public HandleTests()
    {
        applicationState = new Mock<IApplicationState>();
        eventBus = new Mock<IEventBus>();
        cryptographyService = new Mock<ICryptographyService>();

        useCase = new SignMessageUseCase(
            applicationState.Object,
            eventBus.Object,
            cryptographyService.Object);
    }

    [Fact]
    public async Task Handle_WithValidMessageAndSignatureKey_ShouldReturnCommandWorkflowResult()
    {
        // Arrange
        string message = "Test message to sign";
        byte[] expectedSignature = [0x01, 0x02, 0x03, 0x04];

        KeyPair signatureKey = new()
        {
            Id = Guid.NewGuid(),
            PrivateKey = [0x10, 0x20, 0x30, 0x40],
            PublicKey = [0x50, 0x60, 0x70, 0x80]
        };

        SignMessageRequest request = new SignMessageRequest
        {
            Message = message
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(signatureKey);

        cryptographyService
            .Setup(x => x.Sign(signatureKey, message))
            .Returns(expectedSignature);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult<SignMessageResponse>>(result);
    }

    [Fact]
    public async Task Handle_WithValidMessageAndSignatureKey_ShouldUpdateApplicationState()
    {
        // Arrange
        string message = "Test message to sign";
        byte[] expectedSignature = [0x01, 0x02, 0x03, 0x04];

        KeyPair signatureKey = new()
        {
            Id = Guid.NewGuid(),
            PrivateKey = [0x10, 0x20, 0x30, 0x40]
        };

        SignMessageRequest request = new()
        {
            Message = message
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(signatureKey);

        cryptographyService
            .Setup(x => x.Sign(signatureKey, message))
            .Returns(expectedSignature);

        // Act
        await useCase.Handle(request);

        // Assert
        applicationState.VerifySet(x => x.CurrentMessage = message, Times.Once);
        applicationState.VerifySet(x => x.CurrentSignature = expectedSignature, Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidMessageAndSignatureKey_ShouldPublishSignatureCreatedEvent()
    {
        // Arrange
        string message = "Test message to sign";
        byte[] expectedSignature = [0x01, 0x02, 0x03, 0x04];

        KeyPair signatureKey = new()
        {
            Id = Guid.NewGuid(),
            PrivateKey = new byte[] { 0x10, 0x20, 0x30, 0x40 }
        };

        SignMessageRequest request = new()
        {
            Message = message
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(signatureKey);

        cryptographyService
            .Setup(x => x.Sign(signatureKey, message))
            .Returns(expectedSignature);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(
            It.Is<SignatureCreatedEvent>(e => e.Message == message && e.Signature == expectedSignature),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyMessage_ShouldThrowArgumentException()
    {
        // Arrange
        SignMessageRequest request = new()
        {
            Message = ""
        };

        // Act & Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            return useCase.Handle(request);
        });

        Assert.Contains("Message cannot be empty", exception.Message);
        Assert.Equal("Message", exception.ParamName);
    }

    [Fact]
    public async Task Handle_WithNullMessage_ShouldThrowArgumentException()
    {
        // Arrange
        SignMessageRequest request = new SignMessageRequest
        {
            Message = null
        };

        // Act & Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            return useCase.Handle(request);
        });

        Assert.Contains("Message cannot be empty", exception.Message);
        Assert.Equal("Message", exception.ParamName);
    }

    [Fact]
    public async Task Handle_WithWhitespaceMessage_ShouldThrowArgumentException()
    {
        // Arrange
        SignMessageRequest request = new()
        {
            Message = "   "
        };

        // Act & Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
   {
       return useCase.Handle(request);
   });

        Assert.Contains("Message cannot be empty", exception.Message);
        Assert.Equal("Message", exception.ParamName);
    }

    [Fact]
    public async Task Handle_WithNullSignatureKey_ShouldThrowInvalidOperationException()
    {
        // Arrange
        SignMessageRequest request = new()
        {
            Message = "Test message"
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns((KeyPair)null);

        // Act & Assert
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
         {
             return useCase.Handle(request);
         });

        Assert.Equal("No signature key selected", exception.Message);
    }

    [Fact]
    public async Task Handle_WithValidMessage_ShouldCallCryptographyServiceWithCorrectParameters()
    {
        // Arrange
        string message = "Test message to sign";
        byte[] expectedSignature = [0x01, 0x02, 0x03, 0x04];

        KeyPair signatureKey = new()
        {
            Id = Guid.NewGuid(),
            PrivateKey = [0x10, 0x20, 0x30, 0x40]
        };

        SignMessageRequest request = new()
        {
            Message = message
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(signatureKey);

        cryptographyService
            .Setup(x => x.Sign(signatureKey, message))
            .Returns(expectedSignature);

        // Act
        await useCase.Handle(request);

        // Assert
        cryptographyService.Verify(x => x.Sign(signatureKey, message), Times.Once);
    }

    [Theory]
    [InlineData("Short message")]
    [InlineData("This is a longer message that contains multiple words and sentences. It should be handled correctly by the use case.")]
    [InlineData("Special characters: !@#$%^&*()_+-=[]{}|;':\",./<>?")]
    [InlineData("Unicode characters: こんにちは世界 🌍")]
    public async Task Handle_WithVariousMessageFormats_ShouldProcessCorrectly(string message)
    {
        // Arrange
        byte[] expectedSignature = [0x01, 0x02, 0x03, 0x04];

        KeyPair signatureKey = new()
        {
            Id = Guid.NewGuid(),
            PrivateKey = [0x10, 0x20, 0x30, 0x40]
        };

        SignMessageRequest request = new()
        {
            Message = message
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(signatureKey);

        cryptographyService
            .Setup(x => x.Sign(signatureKey, message))
            .Returns(expectedSignature);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.IsType<CommandWorkflowResult<SignMessageResponse>>(result);
    }

    [Fact]
    public async Task Handle_WhenCryptographyServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        string message = "Test message";

        KeyPair signatureKey = new()
        {
            Id = Guid.NewGuid(),
            PrivateKey = [0x10, 0x20, 0x30, 0x40]
        };

        SignMessageRequest request = new()
        {
            Message = message
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(signatureKey);

        cryptographyService
            .Setup(x => x.Sign(signatureKey, message))
            .Throws(new InvalidOperationException("Cryptography error"));

        // Act & Assert
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            return useCase.Handle(request);
        });

        Assert.Equal("Cryptography error", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldNotUpdateApplicationStateIfSigningFails()
    {
        // Arrange
        string message = "Test message";

        KeyPair signatureKey = new()
        {
            Id = Guid.NewGuid(),
            PrivateKey = [0x10, 0x20, 0x30, 0x40]
        };

        SignMessageRequest request = new()
        {
            Message = message
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(signatureKey);

        cryptographyService
            .Setup(x => x.Sign(signatureKey, message))
            .Throws(new InvalidOperationException("Signing failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            return useCase.Handle(request);
        });

        // Verify that application state was not updated
        applicationState.VerifySet(x => x.CurrentMessage = It.IsAny<string>(), Times.Never);
        applicationState.VerifySet(x => x.CurrentSignature = It.IsAny<byte[]>(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldNotPublishEventIfSigningFails()
    {
        // Arrange
        string message = "Test message";

        KeyPair signatureKey = new()
        {
            Id = Guid.NewGuid(),
            PrivateKey = [0x10, 0x20, 0x30, 0x40]
        };

        SignMessageRequest request = new()
        {
            Message = message
        };

        applicationState
            .Setup(x => x.CurrentSignatureKey)
            .Returns(signatureKey);

        cryptographyService
            .Setup(x => x.Sign(signatureKey, message))
            .Throws(new InvalidOperationException("Signing failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
        {
            return useCase.Handle(request);
        });

        // Verify that no event was published
        eventBus.Verify(x => x.PublishAsync(It.IsAny<SignatureCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}