namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;

public class KeyPairDto
{
    public Guid Id { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public byte[] PrivateKey { get; set; }
    
    public byte[] PublicKey { get; set; }
}