namespace DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting.Contracts;

public class KeyPair
{
    public Guid Id { get; set; }
    
    public byte[] PrivateKey { get; set; }
    
    public byte[] PublicKey { get; set; }
}