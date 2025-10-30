using System.Collections.ObjectModel;
using AsyncMediator;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SelectSignatureKey;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

/// <summary>
/// View model for the keys selector control that handles signature key management.
/// </summary>
public class KeysSelectorViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private SignatureKeyViewModel selectedSignatureKey;

    /// <summary>
    /// Gets the collection of available signature keys.
    /// </summary>
    public ObservableCollection<SignatureKeyViewModel> SignatureKeys { get; private set; }

    /// <summary>
    /// Gets or sets the currently selected signature key.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance of the KeysSelectorViewModel class.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    public KeysSelectorViewModel(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Initializes the view model with signature keys data.
    /// </summary>
    /// <param name="signatureKeys">The collection of signature keys to display.</param>
    /// <param name="selectedSignatureKeyId">The ID of the currently selected signature key.</param>
    public void Initialize(IEnumerable<SignatureKeyDto> signatureKeys, Guid? selectedSignatureKeyId)
    {
        Initialization(() =>
        {
            SignatureKeys = new ObservableCollection<SignatureKeyViewModel>(signatureKeys.ToViewModels());
            SelectedSignatureKey = SignatureKeys
                .FirstOrDefault(x => x.Id == selectedSignatureKeyId);
        });
    }

    /// <summary>
    /// Sends a command to select the specified signature key.
    /// </summary>
    /// <param name="signatureKeyId">The ID of the signature key to select.</param>
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
            // For now, we'll just suppress the exception to prevent crashes
            // In a production app, consider logging the error or displaying a user-friendly message
        }
    }
}