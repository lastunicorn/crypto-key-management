namespace DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;

public class InitializeMainResponse
{
    public List<SignatureKeyDto> SignatureKeys { get; set; }

    public Guid? SelectedSignatureKeyId { get; set; }
}
