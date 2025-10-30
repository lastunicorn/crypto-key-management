using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.StateAccess;

public class CurrentSignatureKeyChangedEventArgs : EventArgs
{
    public SignatureKey SignatureKey { get; }

    public CurrentSignatureKeyChangedEventArgs(SignatureKey signatureKey)
    {
        SignatureKey = signatureKey;
    }
}