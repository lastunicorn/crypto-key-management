using System.ComponentModel;
using DustInTheWind.CryptoKeyManagement.Domain;

namespace DustInTheWind.CryptoKeyManagement.Ports.StateAccess;

public class ApplicationState : IApplicationState
{
    private KeyPair currentSignatureKey;
    private string currentMessage = string.Empty;
    private byte[] currentSignature = Array.Empty<byte>();

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

    public byte[] CurrentSignature
    {
        get => currentSignature;
        set
        {
            if (!ReferenceEquals(currentSignature, value) && !currentSignature.SequenceEqual(value ?? Array.Empty<byte>()))
            {
                currentSignature = value ?? Array.Empty<byte>();
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
