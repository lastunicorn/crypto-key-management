using System.Collections.ObjectModel;
using AsyncMediator;
using DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Application.SelectSignatureKey;
using DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

public class MainViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private SignatureKeyViewModel selectedSignatureKey;

    public ObservableCollection<SignatureKeyViewModel> SignatureKeys { get; private set; }

    public SignatureKeyViewModel SelectedSignatureKey
    {
        get => selectedSignatureKey;
        set
        {
            if (selectedSignatureKey != value)
            {
                selectedSignatureKey = value;

                if (!IsInitializing)
                    _ = SelectSignatureKeyAsync(selectedSignatureKey?.Id);

                OnPropertyChanged(nameof(SelectedSignatureKey));
            }
        }
    }

    public SigningPanelViewModel SigningPanelViewModel { get; }

    public MainViewModel(IMediator mediator, SigningPanelViewModel signingPanelViewModel)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        SigningPanelViewModel = signingPanelViewModel ?? throw new ArgumentNullException(nameof(signingPanelViewModel));

        _ = InitializeAsync();
    }

    private Task InitializeAsync()
    {
        return AsInitializationAsync(async () =>
        {
            InitializeMainRequest request = new();
            InitializeMainResponse response = await mediator.Query<InitializeMainRequest, InitializeMainResponse>(request);

            SignatureKeys = new ObservableCollection<SignatureKeyViewModel>(response.SignatureKeys.ToViewModels());
            SelectedSignatureKey = SignatureKeys
                .FirstOrDefault(x => x.Id == response.SelectedSignatureKeyId);
        });
    }

    private async Task SelectSignatureKeyAsync(Guid? signatureKeyId)
    {
        try
        {
            SelectSignatureKeyRequest command = new()
            {
                SignatureKeyId = signatureKeyId
            };
            await mediator.Send(command);
        }
        catch (Exception ex)
        {
            // Handle error appropriately - you might want to show a message to the user
            // For now, just ensure we don't crash the application
            System.Diagnostics.Debug.WriteLine($"Error selecting signature key: {ex.Message}");
        }
    }
}
