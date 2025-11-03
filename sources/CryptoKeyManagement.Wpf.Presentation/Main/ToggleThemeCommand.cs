using System.Windows.Input;
using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.ToggleTheme;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;

public class ToggleThemeCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;

    public ToggleThemeCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public async void Execute(object parameter)
    {
        var request = new ToggleThemeRequest();
        await mediator.Send(request);
    }
}
