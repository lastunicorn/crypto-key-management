using Org.BouncyCastle.Crypto.Parameters;

namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public interface ISignatureRepository
{
    IEnumerable<SignatureKeyInfo> GetAll();

    SignatureKeyInfo GetSignatureById(Guid id);

    Guid SaveSignatureKey(Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey);
}