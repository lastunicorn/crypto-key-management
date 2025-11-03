using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSidebar;

internal class PresentSidebarUseCase : IQuery<PresentSidebarRequest, PresentSidebarResponse>
{
    private readonly ISettingsService settingsService;

    public PresentSidebarUseCase(ISettingsService settingsService)
    {
        this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
    }

    public Task<PresentSidebarResponse> Query(PresentSidebarRequest criteria)
    {
        PresentSidebarResponse response = new()
        {
            ThemeType = settingsService.ThemeType
        };

        return Task.FromResult(response);
    }
}