namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Dialogs;

public interface IDialogService
{
    bool ShowConfirmationDialog(string title, string message, string details = null);
    
    void ShowSuccessDialog(string title, string message);
    
    void ShowErrorDialog(string title, string message);
    
    void ShowInfoDialog(string title, string message);
}
