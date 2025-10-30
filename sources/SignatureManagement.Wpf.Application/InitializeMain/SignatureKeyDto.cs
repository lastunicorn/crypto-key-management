namespace DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;

public class SignatureKeyDto
{
    public Guid Id { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public byte[] PrivateKey { get; set; }
    
    public byte[] PublicKey { get; set; }
}