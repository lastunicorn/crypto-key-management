using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;

/// <summary>
/// View model for the signing panel control that handles message input and signature generation.
/// </summary>
public class SigningPanelViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private string message = string.Empty;
    private string signature = string.Empty;
    private string selectedKeyId = string.Empty;
    private string selectedPrivateKey = string.Empty;
    private string selectedPublicKey = string.Empty;

    /// <summary>
    /// Gets or sets the message to be signed.
    /// </summary>
    public string Message
    {
        get => message;
        set
        {
            if (message != value)
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
    }

    /// <summary>
    /// Gets the generated signature in Base64 format.
    /// </summary>
    public string Signature
    {
        get => signature;
        private set
        {
            if (signature != value)
            {
                signature = value;
                OnPropertyChanged(nameof(Signature));
            }
        }
    }

    /// <summary>
    /// Gets the selected key ID.
    /// </summary>
    public string SelectedKeyId
    {
        get => selectedKeyId;
        private set
        {
            if (selectedKeyId != value)
            {
                selectedKeyId = value;
                OnPropertyChanged(nameof(SelectedKeyId));
            }
        }
    }

    /// <summary>
    /// Gets the selected private key in Base64 format.
    /// </summary>
    public string SelectedPrivateKey
    {
        get => selectedPrivateKey;
        private set
        {
            if (selectedPrivateKey != value)
            {
                selectedPrivateKey = value;
                OnPropertyChanged(nameof(SelectedPrivateKey));
            }
        }
    }

    /// <summary>
    /// Gets the selected public key in Base64 format.
    /// </summary>
    public string SelectedPublicKey
    {
        get => selectedPublicKey;
        private set
        {
            if (selectedPublicKey != value)
            {
                selectedPublicKey = value;
                OnPropertyChanged(nameof(SelectedPublicKey));
            }
        }
    }

    /// <summary>
    /// Command to sign the current message.
    /// </summary>
    public SignMessageCommand SignMessageCommand { get; }

    /// <summary>
    /// Initializes a new instance of the SigningPanelViewModel class.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    public SigningPanelViewModel(IMediator mediator, EventBus eventBus)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        SignMessageCommand = new SignMessageCommand(mediator, signature => Signature = signature);

        eventBus.Subscribe<SignatureKeySelectionChangedEvent>(HandleSignatureKeySelectionChanged);
    }

    private async Task HandleSignatureKeySelectionChanged(SignatureKeySelectionChangedEvent e, CancellationToken token)
    {
        UpdateSelectedKey(e.SignatureKey);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Updates the selected key information to display in the panel.
    /// </summary>
    /// <param name="keyViewModel">The selected key view model, or null if no key is selected.</param>
    private void UpdateSelectedKey(SignatureKeyDto keyViewModel)
    {
        if (keyViewModel == null)
        {
            SelectedKeyId = string.Empty;
            SelectedPrivateKey = string.Empty;
            SelectedPublicKey = string.Empty;
        }
        else
        {
            SelectedKeyId = keyViewModel.Id.ToString();
            SelectedPrivateKey = Convert.ToBase64String(keyViewModel.PrivateKey);
            SelectedPublicKey = Convert.ToBase64String(keyViewModel.PublicKey);
        }
    }
}