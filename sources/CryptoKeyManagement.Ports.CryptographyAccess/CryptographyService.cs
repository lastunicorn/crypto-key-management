using System.Text;
using DustInTheWind.CryptoKeyManagement.Domain;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace DustInTheWind.CryptoKeyManagement.Ports.CryptographyAccess;

public class CryptographyService : ICryptographyService
{
    public byte[] Sign(KeyPair signatureKey, string message)
    {
        Ed25519PrivateKeyParameters privateKey = new(signatureKey.PrivateKey, 0);

        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        Ed25519Signer signer = new();
        signer.Init(true, privateKey);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        return signer.GenerateSignature();
    }
}
