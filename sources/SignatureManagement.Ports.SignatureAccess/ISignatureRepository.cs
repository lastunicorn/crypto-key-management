using Org.BouncyCastle.Crypto.Parameters;

namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public interface ISignatureRepository
{
    List<SignatureKeyInfo> GetAvailableSignatures();

    SignatureKeyInfo GetSignatureById(Guid id);

    Guid SaveSignatureKey(Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey);
}