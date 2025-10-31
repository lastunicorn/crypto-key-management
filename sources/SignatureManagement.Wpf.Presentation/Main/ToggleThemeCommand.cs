
using System.Windows.Input;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Services;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

// Simple RelayCommand implementation
public class ToggleThemeCommand : ICommand
{
    private readonly ThemeSelector themeSelector;

    public ToggleThemeCommand(ThemeSelector themeSelector)
    {
        this.themeSelector = themeSelector ?? throw new ArgumentNullException(nameof(themeSelector));
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

    public void Execute(object parameter)
    {
        themeSelector.ToggleTheme();
    }
}
