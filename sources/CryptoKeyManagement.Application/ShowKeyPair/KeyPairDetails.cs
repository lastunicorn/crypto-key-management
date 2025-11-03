namespace DustInTheWind.CryptoKeyManagement.Application.ShowKeyPair;

public class KeyPairDetails
{
    public Guid Id { get; set; }
    
    public string PrivateKeyValue { get; set; }
    
    public string PublicKeyValue { get; set; }
    
    public DateTime Created { get; set; }
}