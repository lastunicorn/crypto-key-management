using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeysSelector;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPanel;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPage;

public class SigningPageViewModel : ViewModelBase
{
    private readonly IMediator mediator;

    public KeysSelectorViewModel KeysSelectorViewModel { get; }

    public KeyInfoViewModel KeyInfoViewModel { get; }

    public SigningPanelViewModel SigningPanelViewModel { get; }

    public SigningPageViewModel(IMediator mediator, KeysSelectorViewModel keysSelectorViewModel, KeyInfoViewModel keyInfoViewModel, SigningPanelViewModel signingPanelViewModel)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        KeysSelectorViewModel = keysSelectorViewModel ?? throw new ArgumentNullException(nameof(keysSelectorViewModel));
        KeyInfoViewModel = keyInfoViewModel ?? throw new ArgumentNullException(nameof(keyInfoViewModel));
        SigningPanelViewModel = signingPanelViewModel ?? throw new ArgumentNullException(nameof(signingPanelViewModel));

        _ = InitializeAsync();
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