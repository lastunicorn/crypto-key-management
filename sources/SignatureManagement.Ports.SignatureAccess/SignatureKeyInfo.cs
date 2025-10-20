namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public class SignatureKeyInfo
{
    public Guid Id { get; set; }

    public string PrivateKeyPath { get; set; }

    public string PublicKeyPath { get; set; }

    public string PrivateKey { get; set; }

    public string PublicKey { get; set; }
}
