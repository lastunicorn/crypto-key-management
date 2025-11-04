using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.PresentSigningPage.PresentSigningPageUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ICryptoKeyRepository> cryptoKeyRepository;
    private readonly Mock<IApplicationState> applicationState;
    private readonly PresentSigningPageUseCase useCase;

    public ConstructorTests()
    {
        cryptoKeyRepository = new Mock<ICryptoKeyRepository>();
        applicationState = new Mock<IApplicationState>();

        useCase = new PresentSigningPageUseCase(
            cryptoKeyRepository.Object,
            applicationState.Object);
    }

    [Fact]
    public void Constructor_WithNullSignatureKeyRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
           new PresentSigningPageUseCase(null, applicationState.Object));

        Assert.Equal("signatureKeyRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullApplicationState_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            new PresentSigningPageUseCase(cryptoKeyRepository.Object, null));

        Assert.Equal("applicationState", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act
        PresentSigningPageUseCase instance = new(cryptoKeyRepository.Object, applicationState.Object);

        // Assert
        Assert.NotNull(instance);
    }
}