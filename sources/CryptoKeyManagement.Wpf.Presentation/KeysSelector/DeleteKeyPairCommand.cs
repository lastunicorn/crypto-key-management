using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.DeleteKeyPair;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Dialogs;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeysSelector;

public class DeleteKeyPairCommand : System.Windows.Input.ICommand
{
    private readonly IMediator mediator;
    private readonly IDialogService dialogService;

    public DeleteKeyPairCommand(IMediator mediator, IDialogService dialogService)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    }

    public event EventHandler CanExecuteChanged
    {
        add => System.Windows.Input.CommandManager.RequerySuggested += value;
        remove => System.Windows.Input.CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return parameter is Guid || parameter is SignatureKeyViewModel;
    }

    public async void Execute(object parameter)
    {
        try
        {
            Guid keyPairId;
            string displayInfo;

            switch (parameter)
            {
                case Guid guid:
                    keyPairId = guid;
                    displayInfo = $"ID: {guid}";
                    break;

                case SignatureKeyViewModel vm:
                    keyPairId = vm.Id;
                    displayInfo = $"ID: {vm.Id}\nCreated: {vm.CreatedDateText}";
                    break;
                
                default:
                    throw new ArgumentException("Parameter must be a Guid or SignatureKeyViewModel", nameof(parameter));
            }

            bool confirmed = dialogService.ShowConfirmationDialog(
                "Confirm Delete Key",
                "Are you sure you want to delete this signature key?",
                displayInfo);

            if (!confirmed)
                return;

            DeleteKeyPairRequest request = new()
            {
                KeyPairId = keyPairId
            };

            await mediator.Send(request);

            dialogService.ShowSuccessDialog(
                "Delete Successful",
                "Signature key has been successfully deleted.");
        }
        catch (Exception ex)
        {
            dialogService.ShowErrorDialog(
                "Delete Error",
                $"Error deleting key pair: {ex.Message}");
        }
    }
}