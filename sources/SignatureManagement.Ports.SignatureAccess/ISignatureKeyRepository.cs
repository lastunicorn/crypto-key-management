using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public interface ISignatureKeyRepository
{
    IEnumerable<KeyPair> GetAll();

    KeyPair GetById(Guid id);

    Guid Add(byte[] privateKey, byte[] publicKey);

    void Delete(Guid id);
}