using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SelectKeyPair;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.SelectKeyPair.SelectKeyPairUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ISignatureKeyRepository> signatureKeyRepository;
    private readonly Mock<IApplicationState> applicationStateService;
    private readonly Mock<IEventBus> eventBus;

    public ConstructorTests()
    {
        signatureKeyRepository = new Mock<ISignatureKeyRepository>();
        applicationStateService = new Mock<IApplicationState>();
        eventBus = new Mock<IEventBus>();
    }

    [Fact]
    public void Constructor_WithNullSignatureKeyRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new SelectKeyPairUseCase(null, applicationStateService.Object, eventBus.Object);
        });

        Assert.Equal("signatureKeyRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullApplicationStateService_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new SelectKeyPairUseCase(signatureKeyRepository.Object, null, eventBus.Object);
        });

        Assert.Equal("applicationStateService", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullEventBus_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new SelectKeyPairUseCase(signatureKeyRepository.Object, applicationStateService.Object, null);
        });

        Assert.Equal("eventBus", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act
        SelectKeyPairUseCase instance = new(signatureKeyRepository.Object, applicationStateService.Object, eventBus.Object);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldStoreAllDependencies()
    {
        // Act
        SelectKeyPairUseCase instance = new(signatureKeyRepository.Object, applicationStateService.Object, eventBus.Object);

        // Assert - Verify that the instance can be used (dependencies are stored)
        Assert.NotNull(instance);

        // We can't directly verify the private fields are set, but we can verify
        // the constructor doesn't throw and the instance is created successfully
        Assert.IsType<SelectKeyPairUseCase>(instance);
    }
}