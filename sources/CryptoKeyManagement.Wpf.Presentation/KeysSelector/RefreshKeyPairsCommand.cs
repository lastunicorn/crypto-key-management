using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.RefreshKeyPairs;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeysSelector;

/// <summary>
/// Command to refresh the signature keys list.
/// </summary>
public class RefreshKeyPairsCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;
    private bool isExecuting = false;

    public RefreshKeyPairsCommand(IMediator mediator)
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
        return !isExecuting;
    }

    public async void Execute(object parameter)
    {
        if (isExecuting) return;

        try
        {
            isExecuting = true;
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();

            RefreshKeyPairsRequest request = new();
            _ = await mediator.Send(request);

        }
        finally
        {
            isExecuting = false;
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }
    }
}