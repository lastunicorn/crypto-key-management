using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.CreateKeyPair;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.CreateKeyPair.CreateKeyPairUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ICryptoKeyRepository> cryptoKeyRepository;
    private readonly Mock<IEventBus> eventBus;
    private readonly CreateKeyPairUseCase useCase;

    public ConstructorTests()
    {
        cryptoKeyRepository = new Mock<ICryptoKeyRepository>();
        eventBus = new Mock<IEventBus>();

        useCase = new CreateKeyPairUseCase(cryptoKeyRepository.Object, eventBus.Object);
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
            return new CreateKeyPairUseCase(cryptoKeyRepository.Object, null);
        });
    }
}