using AsyncMediator;
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
    public SigningPanelViewModel(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        SignMessageCommand = new SignMessageCommand(mediator, signature => Signature = signature);
    }
}