using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSidebar;
using Moq;

namespace DustInTheWind.CryptoKeyManagement.Tests.Wpf.Application.UseCases.PresentSidebar.PresentSidebarUseCaseTests;

public class QueryTests
{
    private readonly Mock<ISettingsService> settingsService;
    private readonly PresentSidebarUseCase useCase;

    public QueryTests()
    {
        settingsService = new Mock<ISettingsService>();
        useCase = new PresentSidebarUseCase(settingsService.Object);
    }

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
}