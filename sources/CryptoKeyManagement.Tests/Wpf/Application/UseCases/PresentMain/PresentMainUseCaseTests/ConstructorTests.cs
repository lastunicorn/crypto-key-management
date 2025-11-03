using DustInTheWind.CryptoKeyManagement.Ports.SignatureAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.PresentMain.PresentMainUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ISignatureKeyRepository> signatureKeyRepository;
    private readonly Mock<IApplicationState> applicationState;
    private readonly PresentMainUseCase useCase;

    public ConstructorTests()
    {
        signatureKeyRepository = new Mock<ISignatureKeyRepository>();
        applicationState = new Mock<IApplicationState>();

        useCase = new PresentMainUseCase(
            signatureKeyRepository.Object,
            applicationState.Object);
    }

    [Fact]
    public void Constructor_WithNullSignatureKeyRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
           new PresentMainUseCase(null, applicationState.Object));

        Assert.Equal("signatureKeyRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullApplicationState_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            new PresentMainUseCase(signatureKeyRepository.Object, null));

        Assert.Equal("applicationState", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act
        PresentMainUseCase instance = new(signatureKeyRepository.Object, applicationState.Object);

        // Assert
        Assert.NotNull(instance);
    }
}