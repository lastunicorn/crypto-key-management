using System.Reflection;
using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Sidebar;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;

public class MainViewModel : ViewModelBase
{
    private readonly IMediator mediator;

    public string WindowTitle { get; }

    public SidebarViewModel SidebarViewModel { get; }

    public SigningPageViewModel SigningPageViewModel { get; }

    public MainViewModel(IMediator mediator, SidebarViewModel sidebarViewModel, SigningPageViewModel signingPageViewModel)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        SidebarViewModel = sidebarViewModel ?? throw new ArgumentNullException(nameof(sidebarViewModel));
        SigningPageViewModel = signingPageViewModel ?? throw new ArgumentNullException(nameof(signingPageViewModel));

        WindowTitle = GetWindowTitle();
    }

    private static string GetWindowTitle()
    {
        Assembly assembly = Assembly.GetEntryAssembly();
        string version = assembly?.GetName().Version?.ToString(3) ?? "Unknown";
        return $"Crypto Key Management {version}";
    }
}
