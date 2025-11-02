using AsyncMediator;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.DeleteKeyPair;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

public class DeleteKeyPairCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;

    public DeleteKeyPairCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return parameter is Guid || parameter is SignatureKeyViewModel;
    }

    public async void Execute(object parameter)
    {
        if (parameter is not Guid keyPairId)
            throw new ArgumentException("Parameter must be a Guid or SignatureKeyViewModel", nameof(parameter));

        //Guid keyPairId = parameter switch
        //{
        //    Guid guid => guid,
        //    SignatureKeyViewModel x => x.Id,
        //    _ => throw new ArgumentException("Parameter must be a Guid or SignatureKeyViewModel", nameof(parameter))
        //};

        DeleteKeyPairRequest request = new()
        {
            KeyPairId = keyPairId
        };

        await mediator.Send(request);
    }
}