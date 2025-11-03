namespace DustInTheWind.CryptoKeyManagement.Ports.UserAccess;

public class SignatureSummary
{
    public Guid Id { get; set; }

    public byte[] PrivateKey { get; set; }

    public byte[] PublicKey { get; set; }

    public object CreatedDate { get; set; }
}