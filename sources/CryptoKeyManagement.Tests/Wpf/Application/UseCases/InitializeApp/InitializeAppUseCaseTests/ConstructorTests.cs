using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Ports.StateAccess;
using DustInTheWind.CryptoKeyManagement.Ports.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializeApp;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Watchers;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.InitializeApp.InitializeAppUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<IApplicationState> applicationState;
    private readonly ApplicationStateWatcher applicationStateWatcher;
    private readonly Mock<IThemeSelector> themeSelector;
    private readonly Mock<ISettingsService> settingsService;
    private readonly Mock<IEventBus> eventBus;
    private readonly InitializeAppUseCase useCase;

    public ConstructorTests()
    {
        themeSelector = new Mock<IThemeSelector>();
        settingsService = new Mock<ISettingsService>();
        eventBus = new Mock<IEventBus>();
        applicationState = new Mock<IApplicationState>();
        applicationStateWatcher = new ApplicationStateWatcher(applicationState.Object, eventBus.Object);

        useCase = new InitializeAppUseCase(
            applicationStateWatcher,
            themeSelector.Object,
            settingsService.Object,
            eventBus.Object);
    }

    [Fact]
    public void Constructor_WithNullApplicationStateWatcher_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            new InitializeAppUseCase(null, themeSelector.Object, settingsService.Object, eventBus.Object));

        Assert.Equal("applicationStateWatcher", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullThemeSelector_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            new InitializeAppUseCase(applicationStateWatcher, null, settingsService.Object, eventBus.Object));

        Assert.Equal("themeSelector", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullSettingsService_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
            new InitializeAppUseCase(applicationStateWatcher, themeSelector.Object, null, eventBus.Object));

        Assert.Equal("settingsService", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullEventBus_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
          new InitializeAppUseCase(applicationStateWatcher, themeSelector.Object, settingsService.Object, null));

        Assert.Equal("eventBus", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act
        InitializeAppUseCase instance = new(
            applicationStateWatcher,
            themeSelector.Object,
            settingsService.Object,
            eventBus.Object);

        // Assert
        Assert.NotNull(instance);
    }
}