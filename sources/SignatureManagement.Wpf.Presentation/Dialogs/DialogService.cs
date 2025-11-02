using DustInTheWind.SignatureManagement.Ports.WpfUserAccess;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Dialogs;

public class DialogService : IDialogService
{
    private readonly IThemeSelector themeSelector;

    public DialogService(IThemeSelector themeSelector)
    {
        this.themeSelector = themeSelector ?? throw new ArgumentNullException(nameof(themeSelector));
    }

    public bool ShowConfirmationDialog(string title, string message, string details = null)
    {
        var confirmationWindow = new ConfirmationWindow(themeSelector)
        {
            Title = title,
            Message = message,
            KeyInfo = details ?? message
        };

        return confirmationWindow.ShowDialog() == true && confirmationWindow.IsConfirmed;
    }

    public void ShowSuccessDialog(string title, string message)
    {
        var messageWindow = new MessageWindow(themeSelector)
        {
            Title = title,
            Message = message,
            MessageType = MessageTypeEnum.Success
        };
        messageWindow.ShowDialog();
    }

    public void ShowErrorDialog(string title, string message)
    {
        var messageWindow = new MessageWindow(themeSelector)
        {
            Title = title,
            Message = message,
            MessageType = MessageTypeEnum.Error
        };
        messageWindow.ShowDialog();
    }

    public void ShowInfoDialog(string title, string message)
    {
        var messageWindow = new MessageWindow(themeSelector)
        {
            Title = title,
            Message = message,
            MessageType = MessageTypeEnum.Information
        };
        messageWindow.ShowDialog();
    }
}