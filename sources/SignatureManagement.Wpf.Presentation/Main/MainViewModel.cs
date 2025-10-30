using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeysPanel;
using DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

public class MainViewModel : ViewModelBase
{
    private readonly IMediator mediator;

    public KeysPanelViewModel KeysPanelViewModel { get; }

    public KeyInfoViewModel KeyInfoViewModel { get; }

    public SigningPanelViewModel SigningPanelViewModel { get; }

    public MainViewModel(IMediator mediator, KeysPanelViewModel keysPanelViewModel, SigningPanelViewModel signingPanelViewModel, KeyInfoViewModel keyInfoViewModel, EventBus eventBus)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        KeysPanelViewModel = keysPanelViewModel ?? throw new ArgumentNullException(nameof(keysPanelViewModel));
        SigningPanelViewModel = signingPanelViewModel ?? throw new ArgumentNullException(nameof(signingPanelViewModel));
        KeyInfoViewModel = keyInfoViewModel ?? throw new ArgumentNullException(nameof(keyInfoViewModel));

        _ = InitializeAsync();
    }

    private Task InitializeAsync()
    {
        return AsInitializationAsync(async () =>
        {
            InitializeMainRequest request = new();
            InitializeMainResponse response = await mediator.Query<InitializeMainRequest, InitializeMainResponse>(request);

            KeysPanelViewModel.Initialize(response.SignatureKeys, response.SelectedSignatureKeyId);
        });
    }
}
