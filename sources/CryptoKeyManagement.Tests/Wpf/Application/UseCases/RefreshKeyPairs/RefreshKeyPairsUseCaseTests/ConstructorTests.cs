using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.RefreshKeyPairs;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.RefreshKeyPairs.RefreshKeyPairsUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ICryptoKeyRepository> cryptoKeyRepository;
    private readonly Mock<IEventBus> eventBus;

    public ConstructorTests()
    {
        cryptoKeyRepository = new Mock<ICryptoKeyRepository>();
        eventBus = new Mock<IEventBus>();
    }

    [Fact]
    public void Constructor_WithNullCryptoKeyRepository_ShouldThrowArgumentNullException()
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
            new RefreshKeyPairsUseCase(cryptoKeyRepository.Object, null);
        });

        Assert.Equal("eventBus", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act
        RefreshKeyPairsUseCase instance = new(cryptoKeyRepository.Object, eventBus.Object);

        // Assert
        Assert.NotNull(instance);
    }
}