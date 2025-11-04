using DustInTheWind.CryptoKeyManagement.Domain;

namespace DustInTheWind.CryptoKeyManagement.Ports.CryptographyAccess;
public interface ICryptographyService
{
    byte[] Sign(KeyPair keyPair, string message);
}