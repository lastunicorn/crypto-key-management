using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.RefreshKeyPairs;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.RefreshKeyPairs.RefreshKeyPairsUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ISignatureKeyRepository> signatureKeyRepository;
    private readonly Mock<IEventBus> eventBus;

    public ConstructorTests()
    {
        signatureKeyRepository = new Mock<ISignatureKeyRepository>();
        eventBus = new Mock<IEventBus>();
    }

    [Fact]
    public void Constructor_WithNullSignatureKeyRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new RefreshKeyPairsUseCase(null, eventBus.Object);
        });

        Assert.Equal("signatureKeyRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullEventBus_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new RefreshKeyPairsUseCase(signatureKeyRepository.Object, null);
        });

        Assert.Equal("eventBus", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act
        RefreshKeyPairsUseCase instance = new(signatureKeyRepository.Object, eventBus.Object);

        // Assert
        Assert.NotNull(instance);
    }
}