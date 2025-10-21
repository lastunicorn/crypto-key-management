namespace DustInTheWind.SignatureManagement.Application.ShowSignatures;

public class SignatureDetails
{
    public Guid Id { get; set; }
    
    public string PrivateKeyPath { get; set; }
    
    public string PrivateKeyValue { get; set; }
    
    public string PublicKeyPath { get; set; }
    
    public string PublicKeyValue { get; set; }
    
    public DateTime Created { get; set; }
}