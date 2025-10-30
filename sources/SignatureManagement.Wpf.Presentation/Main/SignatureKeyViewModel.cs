namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

public class SignatureKeyViewModel
{
    public Guid Id { get; set; }
    
    public string CreatedDateText { get; set; }
    
    public string PrivateKeyBase64 { get; set; }
    
    public string PublicKeyBase64 { get; set; }
}