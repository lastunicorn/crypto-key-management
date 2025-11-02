using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Ports.SettingsAccess;
using DustInTheWind.SignatureManagement.Ports.StateAccess;
using DustInTheWind.SignatureManagement.Ports.WpfUserAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeApp;
using DustInTheWind.SignatureManagement.Wpf.Application.Watchers;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.InitializeApp;

public class InitializeAppUseCaseTests
{
    private readonly Mock<IApplicationState> applicationState;
    private readonly ApplicationStateWatcher applicationStateWatcher;
    private readonly Mock<IThemeSelector> themeSelector;
    private readonly Mock<ISettingsService> settingsService;
    private readonly Mock<IEventBus> eventBus;
    private readonly InitializeAppUseCase useCase;

    public InitializeAppUseCaseTests()
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
    public async Task Handle_ShouldRetrieveThemeTypeFromSettings()
    {
        // Arrange
        InitializeAppRequest request = new();
        ThemeType expectedThemeType = ThemeType.Dark;
        settingsService
            .Setup(x => x.ThemeType)
            .Returns(expectedThemeType);

        // Act
        await useCase.Handle(request);

        // Assert
        settingsService.Verify(x => x.ThemeType, Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldApplyThemeFromSettings()
    {
        // Arrange
        InitializeAppRequest request = new();
        ThemeType expectedThemeType = ThemeType.Dark;
        settingsService.Setup(x => x.ThemeType).Returns(expectedThemeType);

        // Act
        await useCase.Handle(request);

        // Assert
        themeSelector.Verify(x => x.ApplyTheme(expectedThemeType), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPublishThemeChangedEvent()
    {
        // Arrange
        InitializeAppRequest request = new();
        ThemeType expectedThemeType = ThemeType.Light;
        settingsService
            .Setup(x => x.ThemeType)
            .Returns(expectedThemeType);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.Is<ThemeChangedEvent>(
              x => x.ThemeType == expectedThemeType), CancellationToken.None), Times.Once);
    }

    [Theory]
    [InlineData(ThemeType.Light)]
    [InlineData(ThemeType.Dark)]
    public async Task Handle_WithDifferentThemeTypes_ShouldApplyCorrectTheme(ThemeType themeType)
    {
        // Arrange
        InitializeAppRequest request = new();
        settingsService.Setup(x => x.ThemeType).Returns(themeType);

        // Act
        await useCase.Handle(request);

        // Assert
        themeSelector.Verify(x => x.ApplyTheme(themeType), Times.Once);
        eventBus.Verify(x => x.PublishAsync(It.Is<ThemeChangedEvent>(
            x => x.ThemeType == themeType), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessfulResult()
    {
        // Arrange
        InitializeAppRequest request = new();
        settingsService.Setup(x => x.ThemeType).Returns(ThemeType.Light);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task Handle_ShouldExecuteAllStepsInCorrectOrder()
    {
        // Arrange
        InitializeAppRequest request = new();
        ThemeType expectedThemeType = ThemeType.Dark;
        List<string> callSequence = [];

        settingsService
            .Setup(x => x.ThemeType)
            .Returns(expectedThemeType)
            .Callback(() => callSequence.Add("GetThemeType"));

        themeSelector
            .Setup(x => x.ApplyTheme(It.IsAny<ThemeType>()))
            .Callback(() => callSequence.Add("ApplyTheme"));

        eventBus
            .Setup(x => x.PublishAsync(It.IsAny<ThemeChangedEvent>(), It.IsAny<CancellationToken>()))
            .Callback(() => callSequence.Add("PublishEvent"))
            .Returns(Task.CompletedTask);

        // Act
        await useCase.Handle(request);

        // Assert
        Assert.Equal(new[] { "GetThemeType", "ApplyTheme", "PublishEvent" }, callSequence);
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