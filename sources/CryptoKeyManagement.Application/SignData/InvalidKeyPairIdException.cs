namespace DustInTheWind.CryptoKeyManagement.Application.SignData;

public class InvalidKeyPairIdException : Exception
{
    public InvalidKeyPairIdException(string signatureId)
        : base(string.Format("Invalid signature ID: {0}", signatureId))
    {
    }
}
