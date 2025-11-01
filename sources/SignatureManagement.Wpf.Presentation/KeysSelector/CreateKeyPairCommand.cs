using AsyncMediator;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.CreateKeyPair;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

public class CreateKeyPairCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;

    public CreateKeyPairCommand(IMediator mediator)
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
        return true;
    }

    public async void Execute(object parameter)
    {
        CreateKeyPairRequest command = new();
        await mediator.Send(command);
    }
}