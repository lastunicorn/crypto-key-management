using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;

/// <summary>
/// View model for the signing panel control that handles message input and signature generation.
/// </summary>
public class SigningPanelViewModel : ViewModelBase, IDisposable
{
    private readonly IMediator mediator;
    private readonly EventBus eventBus;
    private string message = string.Empty;
    private string signature = string.Empty;
    private Func<SignatureChangedEvent, CancellationToken, Task> signatureChangedHandler;

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
    /// Command to sign the current message.
    /// </summary>
    public SignMessageCommand SignMessageCommand { get; }

    /// <summary>
    /// Initializes a new instance of the SigningPanelViewModel class.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    /// <param name="eventBus">The event bus for subscribing to events.</param>
    public SigningPanelViewModel(IMediator mediator, EventBus eventBus)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        SignMessageCommand = new SignMessageCommand(mediator);

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        signatureChangedHandler = OnSignatureChanged;
        eventBus.Subscribe(signatureChangedHandler);
    }

    private Task OnSignatureChanged(SignatureChangedEvent @event, CancellationToken cancellationToken)
    {
        Signature = @event.Signature;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (signatureChangedHandler != null)
        {
            eventBus.Unsubscribe(signatureChangedHandler);
            signatureChangedHandler = null;
        }
    }
}