namespace DustInTheWind.SignatureManagement.Application.CreateKeyPair;

public class CreateKeyPairResponse
{
    public Guid KeyId { get; set; }

    public string PrivateKeyPath { get; set; }

    public string PublicKeyPath { get; set; }

    public byte[] PrivateKey { get; set; }

    public byte[] PublicKey { get; set; }
}