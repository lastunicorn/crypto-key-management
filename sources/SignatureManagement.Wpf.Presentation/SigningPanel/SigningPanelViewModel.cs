using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.Events;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;
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
    /// Gets the view model for the key information control.
    /// </summary>
    public KeyInfoViewModel KeyInfoViewModel { get; }

    /// <summary>
    /// Command to sign the current message.
    /// </summary>
    public SignMessageCommand SignMessageCommand { get; }

    /// <summary>
    /// Initializes a new instance of the SigningPanelViewModel class.
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries.</param>
    /// <param name="eventBus">The event bus for handling events.</param>
    public SigningPanelViewModel(IMediator mediator, EventBus eventBus)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        KeyInfoViewModel = new KeyInfoViewModel(eventBus);
        SignMessageCommand = new SignMessageCommand(mediator, signature => Signature = signature);
    }
}