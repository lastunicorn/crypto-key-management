using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.SettingsAccess;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentSidebar;
using Moq;

namespace DustInTheWind.SignatureManagement.Tests.Wpf.Application.UseCases.PresentSidebar;

public class PresentSidebarUseCaseTests
{
    private readonly Mock<ISettingsService> settingsService;
    private readonly PresentSidebarUseCase useCase;

    public PresentSidebarUseCaseTests()
    {
        settingsService = new Mock<ISettingsService>();
        useCase = new PresentSidebarUseCase(settingsService.Object);
    }

    #region Constructor Tests

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

    #endregion

    #region Query Method Tests

    [Fact]
    public async Task Query_WithNullRequest_ShouldNotThrow()
    {
        // Arrange
        settingsService
           .Setup(x => x.ThemeType)
           .Returns(ThemeType.Light);

        // Act & Assert
        PresentSidebarResponse response = await useCase.Query(null);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Query_ShouldReturnResponseWithThemeTypeFromSettings()
    {
        // Arrange
        PresentSidebarRequest request = new();
        ThemeType expectedThemeType = ThemeType.Dark;

        settingsService
            .Setup(x => x.ThemeType)
            .Returns(expectedThemeType);

        // Act
        PresentSidebarResponse response = await useCase.Query(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedThemeType, response.ThemeType);
    }

    [Fact]
    public async Task Query_WithLightTheme_ShouldReturnLightThemeType()
    {
        // Arrange
        PresentSidebarRequest request = new();
        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Light);

        // Act
        PresentSidebarResponse response = await useCase.Query(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(ThemeType.Light, response.ThemeType);
    }

    [Fact]
    public async Task Query_WithDarkTheme_ShouldReturnDarkThemeType()
    {
        // Arrange
        PresentSidebarRequest request = new();
        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Dark);

        // Act
        PresentSidebarResponse response = await useCase.Query(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(ThemeType.Dark, response.ThemeType);
    }

    [Fact]
    public async Task Query_ShouldAccessSettingsServiceThemeTypeProperty()
    {
        // Arrange
        PresentSidebarRequest request = new();
        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Light);

        // Act
        await useCase.Query(request);

        // Assert
        settingsService.Verify(x => x.ThemeType, Times.Once);
    }

    [Fact]
    public async Task Query_ShouldReturnCompletedTask()
    {
        // Arrange
        PresentSidebarRequest request = new();
        settingsService
            .Setup(x => x.ThemeType)
            .Returns(ThemeType.Light);

        // Act
        Task<PresentSidebarResponse> responseTask = useCase.Query(request);

        // Assert
        Assert.True(responseTask.IsCompleted);
        PresentSidebarResponse response = await responseTask;
        Assert.NotNull(response);
    }

    [Fact]
    public async Task Query_CalledMultipleTimes_ShouldReturnConsistentResults()
    {
        // Arrange
        PresentSidebarRequest request = new();
        ThemeType expectedThemeType = ThemeType.Dark;
        settingsService
            .Setup(x => x.ThemeType)
            .Returns(expectedThemeType);

        // Act
        PresentSidebarResponse response1 = await useCase.Query(request);
        PresentSidebarResponse response2 = await useCase.Query(request);

        // Assert
        Assert.NotNull(response1);
        Assert.NotNull(response2);
        Assert.Equal(expectedThemeType, response1.ThemeType);
        Assert.Equal(expectedThemeType, response2.ThemeType);
        Assert.Equal(response1.ThemeType, response2.ThemeType);
    }

    [Fact]
    public async Task Query_WhenSettingsServiceThemeTypeChanges_ShouldReturnUpdatedThemeType()
    {
        // Arrange
        PresentSidebarRequest request = new();

        settingsService
            .SetupSequence(x => x.ThemeType)
            .Returns(ThemeType.Light)
            .Returns(ThemeType.Dark);

        // Act
        PresentSidebarResponse response1 = await useCase.Query(request);
        PresentSidebarResponse response2 = await useCase.Query(request);

        // Assert
        Assert.NotNull(response1);
        Assert.NotNull(response2);
        Assert.Equal(ThemeType.Light, response1.ThemeType);
        Assert.Equal(ThemeType.Dark, response2.ThemeType);
    }

    #endregion
}