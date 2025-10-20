namespace DustInTheWind.SignatureManagement.Application.CreateSignature;

public class CreateSignatureResponse
{
    public Guid KeyId { get; set; }

    public string PrivateKeyPath { get; set; }

    public string PublicKeyPath { get; set; }

    public byte[] PrivateKey { get; set; }

    public byte[] PublicKey { get; set; }
}