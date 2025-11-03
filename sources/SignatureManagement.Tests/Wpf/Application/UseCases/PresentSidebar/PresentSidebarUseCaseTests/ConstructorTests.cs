using DustInTheWind.SignatureManagement.Ports.SettingsAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentSidebar;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.PresentSidebaraa;

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