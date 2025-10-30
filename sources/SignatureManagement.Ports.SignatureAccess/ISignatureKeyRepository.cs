using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public interface ISignatureKeyRepository
{
    IEnumerable<SignatureKey> GetAll();

    SignatureKey GetById(Guid id);

    Guid Add(byte[] privateKey, byte[] publicKey);
}