namespace DustInTheWind.CryptoKeyManagement.Domain;

public class KeyPair
{
    public Guid Id { get; set; }

    public string PrivateKeyPath { get; set; }

    public string PublicKeyPath { get; set; }

    public byte[] PrivateKey { get; set; }

    public byte[] PublicKey { get; set; }

    public DateTime CreatedDate { get; set; }
}
