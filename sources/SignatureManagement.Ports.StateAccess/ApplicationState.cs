namespace DustInTheWind.SignatureManagement.Ports.StateAccess;

public class ApplicationState : IApplicationState
{
    private Guid? selectedSignatureKeyId;

    public Guid? SelectedSignatureKeyId
    {
        get => selectedSignatureKeyId;
        set
        {
            if (selectedSignatureKeyId != value)
            {
                selectedSignatureKeyId = value;
                OnSelectedSignatureKeyChanged(value);
            }
        }
    }

    public event EventHandler<Guid?> SelectedSignatureKeyChanged;

    private void OnSelectedSignatureKeyChanged(Guid? signatureKeyId)
    {
        SelectedSignatureKeyChanged?.Invoke(this, signatureKeyId);
    }
}