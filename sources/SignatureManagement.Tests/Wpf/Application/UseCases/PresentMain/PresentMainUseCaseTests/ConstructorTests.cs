using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.PresentMain.PresentMainUseCaseTests;

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