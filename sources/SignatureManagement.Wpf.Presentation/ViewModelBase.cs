using System.ComponentModel;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    protected bool IsInitializing { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void AsInitialization(Action action)
    {
        IsInitializing = true;

        try
        {
            action?.Invoke();
        }
        finally
        {
            IsInitializing = false;
        }
    }

    protected Task AsInitializationAsync(Func<Task> action)
    {
        IsInitializing = true;

        try
        {
            return action?.Invoke();
        }
        finally
        {
            IsInitializing = false;
        }
    }
}
