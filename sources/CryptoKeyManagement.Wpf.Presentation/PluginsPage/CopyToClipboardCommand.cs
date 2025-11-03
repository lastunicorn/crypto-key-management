using System.Windows;
using System.Windows.Input;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;

public class CopyToClipboardCommand : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        string text = parameter?.ToString();

        if (!string.IsNullOrEmpty(text))
        {
            try
            {
                Clipboard.SetText(text);
            }
            catch
            {
                // Silently handle clipboard access failures
            }
        }
    }
}