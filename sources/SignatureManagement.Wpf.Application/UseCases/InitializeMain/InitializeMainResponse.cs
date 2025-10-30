namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

public class InitializeMainResponse
{
    public List<SignatureKeyDto> SignatureKeys { get; set; }

    public Guid? SelectedSignatureKeyId { get; set; }
}
