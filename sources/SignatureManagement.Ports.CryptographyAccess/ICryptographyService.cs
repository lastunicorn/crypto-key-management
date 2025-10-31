using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.CryptographyAccess;
public interface ICryptographyService
{
    byte[] Sign(KeyPair signatureKey, string message);
}