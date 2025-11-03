using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;
using DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Sidebar;
using System.Reflection;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

public class MainViewModel : ViewModelBase
{
    private readonly IMediator mediator;

    public string WindowTitle { get; }

    public KeysSelectorViewModel KeysSelectorViewModel { get; }

    public KeyInfoViewModel KeyInfoViewModel { get; }

    public SigningPanelViewModel SigningPanelViewModel { get; }

    public SidebarViewModel SidebarViewModel { get; }

    public MainViewModel(IMediator mediator, IEventBus eventBus,
        KeysSelectorViewModel keysSelectorViewModel, SigningPanelViewModel signingPanelViewModel,
        KeyInfoViewModel keyInfoViewModel, SidebarViewModel sidebarViewModel)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        KeysSelectorViewModel = keysSelectorViewModel ?? throw new ArgumentNullException(nameof(keysSelectorViewModel));
        SigningPanelViewModel = signingPanelViewModel ?? throw new ArgumentNullException(nameof(signingPanelViewModel));
        KeyInfoViewModel = keyInfoViewModel ?? throw new ArgumentNullException(nameof(keyInfoViewModel));
        SidebarViewModel = sidebarViewModel ?? throw new ArgumentNullException(nameof(sidebarViewModel));

        WindowTitle = GetWindowTitle();

        _ = InitializeAsync();
    }

    private static string GetWindowTitle()
    {
        Assembly assembly = Assembly.GetEntryAssembly();
        string version = assembly?.GetName().Version?.ToString(3) ?? "Unknown";
        return $"Signature Keys Management {version}";
    }

    private Task InitializeAsync()
    {
        return AsInitializationAsync(async () =>
        {
            PresentMainRequest request = new();
            PresentMainResponse response = await mediator.Query<PresentMainRequest, PresentMainResponse>(request);

            KeysSelectorViewModel.Initialize(response.SignatureKeys, response.SelectedSignatureKeyId);
        });
    }
}
