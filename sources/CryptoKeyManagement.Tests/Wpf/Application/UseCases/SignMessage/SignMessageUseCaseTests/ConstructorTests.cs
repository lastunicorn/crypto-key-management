using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptographyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SignMessage;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.SignMessage.SignMessageUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<IApplicationState> applicationState;
    private readonly Mock<IEventBus> eventBus;
    private readonly Mock<ICryptographyService> cryptographyService;

    public ConstructorTests()
    {
        applicationState = new Mock<IApplicationState>();
        eventBus = new Mock<IEventBus>();
        cryptographyService = new Mock<ICryptographyService>();
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act
        SignMessageUseCase useCase = new(
            applicationState.Object,
            eventBus.Object,
            cryptographyService.Object);

        // Assert
        Assert.NotNull(useCase);
    }

    [Fact]
    public void Constructor_WithNullApplicationState_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            return new SignMessageUseCase(
                null,
                eventBus.Object,
                cryptographyService.Object);
        });
    }

    [Fact]
    public void Constructor_WithNullEventBus_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            return new SignMessageUseCase(
                applicationState.Object,
                null,
                cryptographyService.Object);
        });
    }

    [Fact]
    public void Constructor_WithNullCryptographyService_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            return new SignMessageUseCase(
                applicationState.Object,
                eventBus.Object,
                null);
        });
    }
}