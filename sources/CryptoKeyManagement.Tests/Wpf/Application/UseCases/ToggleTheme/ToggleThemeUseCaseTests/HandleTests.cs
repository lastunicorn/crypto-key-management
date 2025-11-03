using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Ports.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.ToggleTheme;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.ToggleTheme.ToggleThemeUseCaseTests;

public class HandleTests
{
    private readonly Mock<ISettingsService> settingsService;
    private readonly Mock<IEventBus> eventBus;
    private readonly Mock<IThemeSelector> themeSelector;
    private readonly ToggleThemeUseCase useCase;

    public HandleTests()
    {
        settingsService = new Mock<ISettingsService>();
        eventBus = new Mock<IEventBus>();
        themeSelector = new Mock<IThemeSelector>();

        useCase = new ToggleThemeUseCase(
            settingsService.Object,
            eventBus.Object,
            themeSelector.Object);
    }

    [Fact]
    public async Task Handle_WithLightTheme_ShouldToggleToDarkTheme()
    {
        // Arrange
        ToggleThemeRequest request = new();
        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Light);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        settingsService.VerifySet(x => x.ThemeType = ThemeType.Dark, Times.Once);
        themeSelector.Verify(x => x.ApplyTheme(ThemeType.Dark), Times.Once);
        eventBus.Verify(x => x.PublishAsync(It.Is<ThemeChangedEvent>(e => e.ThemeType == ThemeType.Dark), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDarkTheme_ShouldToggleToLightTheme()
    {
        // Arrange
        ToggleThemeRequest request = new();

        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Dark);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        settingsService.VerifySet(x => x.ThemeType = ThemeType.Light, Times.Once);
        themeSelector.Verify(x => x.ApplyTheme(ThemeType.Light), Times.Once);
        eventBus.Verify(x => x.PublishAsync(It.Is<ThemeChangedEvent>(e => e.ThemeType == ThemeType.Light), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithUnknownTheme_ShouldDefaultToLightTheme()
    {
        // Arrange
        ToggleThemeRequest request = new();

        settingsService
            .Setup(x => x.ThemeType)
            .Returns((ThemeType)999); // Invalid enum value

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        settingsService.VerifySet(x => x.ThemeType = ThemeType.Light, Times.Once);
        themeSelector.Verify(x => x.ApplyTheme(ThemeType.Light), Times.Once);
        eventBus.Verify(x => x.PublishAsync(It.Is<ThemeChangedEvent>(e => e.ThemeType == ThemeType.Light), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnOkResult()
    {
        // Arrange
        ToggleThemeRequest request = new();

        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Light);

        // Act
        ICommandWorkflowResult result = await useCase.Handle(request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CommandWorkflowResult>(result);
    }

    [Fact]
    public async Task Handle_ShouldCallThemeSelectorApplyTheme()
    {
        // Arrange
        ToggleThemeRequest request = new();

        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Light);

        // Act
        await useCase.Handle(request);

        // Assert
        themeSelector.Verify(x => x.ApplyTheme(ThemeType.Dark), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPublishThemeChangedEvent()
    {
        // Arrange
        ToggleThemeRequest request = new();

        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Light);

        // Act
        await useCase.Handle(request);

        // Assert
        eventBus.Verify(x => x.PublishAsync(It.Is<ThemeChangedEvent>(e => e.ThemeType == ThemeType.Dark), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateSettingsServiceThemeType()
    {
        // Arrange
        ToggleThemeRequest request = new();

        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Dark);

        // Act
        await useCase.Handle(request);

        // Assert
        settingsService.VerifySet(x => x.ThemeType = ThemeType.Light, Times.Once);
    }
}