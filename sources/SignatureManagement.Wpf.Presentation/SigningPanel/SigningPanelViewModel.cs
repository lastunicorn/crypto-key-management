using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.SignatureFormatting;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;

/// <summary>
/// View model for the signing panel control that handles message input and signature generation.
/// </summary>
public class SigningPanelViewModel : ViewModelBase, IDisposable
{
    private bool isDisposed;
    private readonly IEventBus eventBus;
    private readonly ISignatureFormatter signatureFormatter;
    private string message = string.Empty;
    private string signature = string.Empty;

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

    public SignMessageCommand SignMessageCommand { get; }

    public SigningPanelViewModel(IEventBus eventBus, SignMessageCommand signMessageCommand, ISignatureFormatter signatureFormatter)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        SignMessageCommand = signMessageCommand ?? throw new ArgumentNullException(nameof(signMessageCommand));
        this.signatureFormatter = signatureFormatter ?? throw new ArgumentNullException(nameof(signatureFormatter));

        eventBus.Subscribe<SignatureCreatedEvent>(HandleSignatureChanged);
    }

    private Task HandleSignatureChanged(SignatureCreatedEvent @event, CancellationToken cancellationToken)
    {
        // Use the formatter to convert byte[] signature to string for display
        Signature = signatureFormatter.FormatSignature(@event.Signature);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool isDisposing)
    {
        if (isDisposed)
            return;

        eventBus.UnsubscribeAllForMe();

        isDisposed = true;
    }

    ~SigningPanelViewModel()
    {
        Dispose(false);
    }
}