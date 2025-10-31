namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

public class KeyPairDto
{
    public Guid Id { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public byte[] PrivateKey { get; set; }
    
    public byte[] PublicKey { get; set; }
}