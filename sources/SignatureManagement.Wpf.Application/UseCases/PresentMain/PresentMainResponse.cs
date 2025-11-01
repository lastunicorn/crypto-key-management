namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;

public class PresentMainResponse
{
    public List<KeyPairDto> SignatureKeys { get; set; }

    public Guid? SelectedSignatureKeyId { get; set; }
}
