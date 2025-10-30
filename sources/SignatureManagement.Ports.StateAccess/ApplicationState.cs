using System.ComponentModel;
using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.StateAccess;

public class ApplicationState : IApplicationState
{
    private SignatureKey currentSignatureKey;

    public SignatureKey CurrentSignatureKey
    {
        get => currentSignatureKey;
        set
        {
            if (currentSignatureKey != value)
            {
                currentSignatureKey = value;
                OnPropertyChanged(nameof(CurrentSignatureKey));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
