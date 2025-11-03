using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.DeleteKeyPair;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.DeleteKeyPair.DeleteKeyPairUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ISignatureKeyRepository> signatureKeyRepository;
    private readonly Mock<IApplicationState> applicationState;
    private readonly Mock<IEventBus> eventBus;
    private readonly DeleteKeyPairUseCase useCase;

    public ConstructorTests()
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
}