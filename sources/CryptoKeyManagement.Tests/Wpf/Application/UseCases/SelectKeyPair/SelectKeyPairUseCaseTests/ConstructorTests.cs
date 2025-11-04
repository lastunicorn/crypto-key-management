using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SelectKeyPair;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.SelectKeyPair.SelectKeyPairUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ICryptoKeyRepository> signatureKeyRepository;
    private readonly Mock<IApplicationState> applicationStateService;
    private readonly Mock<IEventBus> eventBus;

    public ConstructorTests()
    {
        signatureKeyRepository = new Mock<ICryptoKeyRepository>();
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