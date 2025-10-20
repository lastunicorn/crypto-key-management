using Org.BouncyCastle.Crypto.Parameters;

namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public interface ISignatureRepository
{
    List<SignatureKeyInfo> GetAvailableSignatures();

    Guid SaveSignatureKey(Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey, out string privateKeyPath, out string publicKeyPath);
}