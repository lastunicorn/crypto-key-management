using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;

/// <summary>
/// View model for the key information control that displays the currently selected signature key details.
/// </summary>
public class KeyInfoViewModel : ViewModelBase
{
    private string selectedKeyId = string.Empty;
    private string selectedPrivateKey = string.Empty;
    private string selectedPublicKey = string.Empty;

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
    /// Initializes a new instance of the KeyInfoViewModel class.
    /// </summary>
    /// <param name="eventBus">The event bus for handling signature key selection changes.</param>
    public KeyInfoViewModel(EventBus eventBus)
    {
        eventBus.Subscribe<KeyPairSelectionChangedEvent>(HandleSignatureKeySelectionChanged);
    }

    /// <summary>
    /// Handles the signature key selection changed event.
    /// </summary>
    /// <param name="e">The event containing the selected signature key information.</param>
    /// <param name="token">Cancellation token.</param>
    private async Task HandleSignatureKeySelectionChanged(KeyPairSelectionChangedEvent e, CancellationToken token)
    {
        UpdateSelectedKey(e.SignatureKey);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Updates the selected key information to display in the control.
    /// </summary>
    /// <param name="keyDto">The selected key DTO, or null if no key is selected.</param>
    public void UpdateSelectedKey(SignatureKeyDto keyDto)
    {
        if (keyDto == null)
        {
            SelectedKeyId = string.Empty;
            SelectedPrivateKey = string.Empty;
            SelectedPublicKey = string.Empty;
        }
        else
        {
            SelectedKeyId = keyDto.Id.ToString();
            SelectedPrivateKey = Convert.ToBase64String(keyDto.PrivateKey);
            SelectedPublicKey = Convert.ToBase64String(keyDto.PublicKey);
        }
    }
}