using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Ports.WpfUserAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.ToggleTheme;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.ToggleTheme.ToggleThemeUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ISettingsService> settingsService;
    private readonly Mock<IEventBus> eventBus;
    private readonly Mock<IThemeSelector> themeSelector;

    public ConstructorTests()
    {
        settingsService = new Mock<ISettingsService>();
        eventBus = new Mock<IEventBus>();
        themeSelector = new Mock<IThemeSelector>();
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act
        ToggleThemeUseCase useCase = new(
            settingsService.Object,
            eventBus.Object,
            themeSelector.Object);

        // Assert
        Assert.NotNull(useCase);
    }

    [Fact]
    public void Constructor_WithNullSettingsService_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            return new ToggleThemeUseCase(
                null,
                eventBus.Object,
                themeSelector.Object);
        });
    }

    [Fact]
    public void Constructor_WithNullEventBus_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            return new ToggleThemeUseCase(
                settingsService.Object,
                null,
                themeSelector.Object);
        });
    }

    [Fact]
    public void Constructor_WithNullThemeSelector_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            return new ToggleThemeUseCase(
                settingsService.Object,
                eventBus.Object,
                null);
        });
    }
}