using AsyncMediator;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SignMessage;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;

public class SignMessageCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;

    public SignMessageCommand(IMediator mediator)
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
        string message = parameter as string;

        return !string.IsNullOrWhiteSpace(message);
    }

    public async void Execute(object parameter)
    {
        string message = parameter as string;

        if (string.IsNullOrWhiteSpace(message))
            return;

        SignMessageRequest command = new()
        {
            Message = message
        };

        await mediator.Send(command);
    }
}