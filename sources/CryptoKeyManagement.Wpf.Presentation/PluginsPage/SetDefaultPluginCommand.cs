using System.Windows.Input;
using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SetDefaultPlugin;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;

public class SetDefaultPluginCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;

    public SetDefaultPluginCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return parameter is Guid;
    }

    public async void Execute(object parameter)
    {
        if (parameter is not Guid pluginId)
            return;

        SetDefaultPluginRequest request = new()
        {
            PluginId = pluginId
        };
        await mediator.Send(request);
    }
}