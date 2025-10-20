namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public class SignatureKeyInfo
{
    public Guid Id { get; set; }

    public string PrivateKeyPath { get; set; }

    public string PublicKeyPath { get; set; }
}
