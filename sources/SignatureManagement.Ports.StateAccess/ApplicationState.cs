using System.ComponentModel;
using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.StateAccess;

public class ApplicationState : IApplicationState
{
    private KeyPair currentSignatureKey;
    private string currentMessage = string.Empty;
    private string currentSignature = string.Empty;

    public KeyPair CurrentSignatureKey
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

    public string CurrentMessage
    {
        get => currentMessage;
        set
        {
            if (currentMessage != value)
            {
                currentMessage = value;
                OnPropertyChanged(nameof(CurrentMessage));
            }
        }
    }

    public string CurrentSignature
    {
        get => currentSignature;
        set
        {
            if (currentSignature != value)
            {
                currentSignature = value;
                OnPropertyChanged(nameof(CurrentSignature));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
