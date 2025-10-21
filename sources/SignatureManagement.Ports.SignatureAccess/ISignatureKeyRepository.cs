using Org.BouncyCastle.Crypto.Parameters;

namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public interface ISignatureKeyRepository
{
    IEnumerable<SignatureKey> GetAll();

    SignatureKey GetById(Guid id);

    Guid Add(Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey);
}