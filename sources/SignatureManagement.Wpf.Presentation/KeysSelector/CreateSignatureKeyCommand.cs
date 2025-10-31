using AsyncMediator;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.CreateSignatureKey;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

public class CreateSignatureKeyCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;

    public CreateSignatureKeyCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public event EventHandler CanExecuteChanged
    {
        add => System.Windows.Input.CommandManager.RequerySuggested += value;
        remove => System.Windows.Input.CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return true; // Always allow creating a new key
    }

    public async void Execute(object parameter)
    {
        try
        {
            CreateSignatureKeyRequest command = new();
            ICommandWorkflowResult result = await mediator.Send(command);
        }
        catch (Exception ex)
        {
            // Handle error appropriately - you might want to show a message to the user
            // For now, just ensure we don't crash the application
            System.Diagnostics.Debug.WriteLine($"Error creating signature key: {ex.Message}");
            // TODO: Consider showing a user-friendly error message
        }
    }
}