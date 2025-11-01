using System.Collections.ObjectModel;
using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.RefreshKeyPairs;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SelectKeyPair;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

/// <summary>
/// View model for the keys selector control that handles signature key management.
/// </summary>
public class KeysSelectorViewModel : ViewModelBase, IDisposable
{
    private readonly IMediator mediator;
    private readonly EventBus eventBus;
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
    /// Gets the command to create a new signature key.
    /// </summary>
    public CreateKeyPairCommand CreateKeyPairCommand { get; private set; }

    /// <summary>
    /// Gets the command to refresh the signature keys list.
    /// </summary>
    public RefreshKeyPairsCommand RefreshKeyPairsCommand { get; private set; }

    /// <summary>
    /// Initializes a new instance of the KeysSelectorViewModel class.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    public KeysSelectorViewModel(IMediator mediator, EventBus eventBus,
        CreateKeyPairCommand createKeyPairCommand, RefreshKeyPairsCommand refreshKeyPairsCommand)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        CreateKeyPairCommand = createKeyPairCommand ?? throw new ArgumentNullException(nameof(createKeyPairCommand));
        RefreshKeyPairsCommand = refreshKeyPairsCommand ?? throw new ArgumentNullException(nameof(refreshKeyPairsCommand));

        eventBus.Subscribe<KeyPairCreatedEvent>(HandleSignatureKeyCreatedEvent);
        eventBus.Subscribe<KeyPairsRefreshEvent>(HandleKeyPairsRefreshEvent);
    }

    /// <summary>
    /// Initializes the view model with signature keys data.
    /// </summary>
    /// <param name="signatureKeys">The collection of signature keys to display.</param>
    /// <param name="selectedSignatureKeyId">The ID of the currently selected signature key.</param>
    public void Initialize(IEnumerable<KeyPairDto> signatureKeys, Guid? selectedSignatureKeyId)
    {
        Initialization(() =>
        {
            IEnumerable<SignatureKeyViewModel> keyViewModels = signatureKeys
                .OrderBy(x => x.CreatedDate)
                .ToViewModels();

            SignatureKeys = new ObservableCollection<SignatureKeyViewModel>(keyViewModels);

            SelectedSignatureKey = SignatureKeys
                .FirstOrDefault(x => x.Id == selectedSignatureKeyId);
        });
    }

    /// <summary>
    /// Handles the key creation completion by refreshing the keys list.
    /// </summary>
    private Task HandleSignatureKeyCreatedEvent(KeyPairCreatedEvent ev, CancellationToken cancellationToken)
    {
        RefreshKeyPairsRequest request = new();
        return mediator.Send(request);
    }

    /// <summary>
    /// Handles the key pairs refresh event by updating the UI with the new data.
    /// </summary>
    /// <param name="ev">The refresh event containing the updated signature keys.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private Task HandleKeyPairsRefreshEvent(KeyPairsRefreshEvent ev, CancellationToken cancellationToken)
    {
        Guid? currentSelectionId = SelectedSignatureKey?.Id;

        SignatureKeys.Clear();

        IEnumerable<SignatureKeyViewModel> keyViewModels = ev.SignatureKeys
            .OrderBy(x => x.CreatedDate)
            .ToViewModels();

        foreach (SignatureKeyViewModel keyViewModel in keyViewModels)
            SignatureKeys.Add(keyViewModel);

        SelectedSignatureKey = currentSelectionId == null
            ? null
            : SignatureKeys.FirstOrDefault(x => x.Id == currentSelectionId);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Sends a command to select the specified signature key.
    /// </summary>
    /// <param name="signatureKeyId">The ID of the signature key to select.</param>
    private async Task SelectSignatureKeyAsync(Guid? signatureKeyId)
    {
        SelectKeyPairRequest command = new()
        {
            SignatureKeyId = signatureKeyId
        };
        await mediator.Send(command);
    }

    public void Dispose()
    {
        eventBus.UnsubscribeAllForMe();
    }
}