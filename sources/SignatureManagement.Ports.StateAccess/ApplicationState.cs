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

                CurrentSignatureKeyChangedEventArgs eventArgs = new(value);
                OnCurrentSignatureKeyChanged(eventArgs);
            }
        }
    }

    public event EventHandler<CurrentSignatureKeyChangedEventArgs> CurrentSignatureKeyChanged;

    private void OnCurrentSignatureKeyChanged(CurrentSignatureKeyChangedEventArgs eventArgs)
    {
        CurrentSignatureKeyChanged?.Invoke(this, eventArgs);
    }
}
