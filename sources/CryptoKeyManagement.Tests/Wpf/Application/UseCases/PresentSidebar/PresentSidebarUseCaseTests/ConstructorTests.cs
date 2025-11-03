using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSidebar;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.PresentSidebar.PresentSidebarUseCaseTests;

public class ConstructorTests
{
    private readonly Mock<ISettingsService> settingsService;

    public ConstructorTests()
    {
        settingsService = new Mock<ISettingsService>();
    }

    [Fact]
    public void Constructor_WithNullSettingsService_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new PresentSidebarUseCase(null);
        });

        Assert.Equal("settingsService", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidSettingsService_ShouldCreateInstance()
    {
        // Act
        PresentSidebarUseCase instance = new(settingsService.Object);

        // Assert
        Assert.NotNull(instance);
    }
}