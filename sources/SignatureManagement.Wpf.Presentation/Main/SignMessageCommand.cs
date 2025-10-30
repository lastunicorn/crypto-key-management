using AsyncMediator;
using DustInTheWind.SignatureManagement.Wpf.Application.SignMessage;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

public class SignMessageCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;
    private readonly Func<SignatureKeyViewModel> getSelectedSignatureKey;
    private readonly Action<string> setSignature;

    public SignMessageCommand(
        IMediator mediator,
        Func<SignatureKeyViewModel> getSelectedSignatureKey,
        Action<string> setSignature)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.getSelectedSignatureKey = getSelectedSignatureKey ?? throw new ArgumentNullException(nameof(getSelectedSignatureKey));
        this.setSignature = setSignature ?? throw new ArgumentNullException(nameof(setSignature));
    }

    public event EventHandler CanExecuteChanged
    {
        add => System.Windows.Input.CommandManager.RequerySuggested += value;
        remove => System.Windows.Input.CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        string message = parameter as string;
        var selectedKey = getSelectedSignatureKey();

        return !string.IsNullOrWhiteSpace(message) && selectedKey != null;
    }

    public async void Execute(object parameter)
    {
        try
        {
            string message = parameter as string;
            var selectedKey = getSelectedSignatureKey();

            if (string.IsNullOrWhiteSpace(message) || selectedKey == null)
                return;

            SignMessageRequest command = new()
            {
                Message = message
            };

            ICommandWorkflowResult result = await mediator.Send(command);
            SignMessageResponse response = result.Result<SignMessageResponse>();

            setSignature(response.Signature);
        }
        catch (Exception ex)
        {
            // Handle error appropriately - you might want to show a message to the user
            // For now, just ensure we don't crash the application
            System.Diagnostics.Debug.WriteLine($"Error signing message: {ex.Message}");
            setSignature($"Error: {ex.Message}");
        }
    }
}