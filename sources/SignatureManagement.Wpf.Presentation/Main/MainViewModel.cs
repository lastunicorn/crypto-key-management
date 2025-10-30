using AsyncMediator;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeysPanel;
using DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

public class MainViewModel : ViewModelBase
{
    private readonly IMediator mediator;

    public KeysPanelViewModel KeysPanelViewModel { get; }

    public SigningPanelViewModel SigningPanelViewModel { get; }

    public MainViewModel(IMediator mediator, SigningPanelViewModel signingPanelViewModel, KeysPanelViewModel keysPanelViewModel)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        SigningPanelViewModel = signingPanelViewModel ?? throw new ArgumentNullException(nameof(signingPanelViewModel));
        KeysPanelViewModel = keysPanelViewModel ?? throw new ArgumentNullException(nameof(keysPanelViewModel));

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
