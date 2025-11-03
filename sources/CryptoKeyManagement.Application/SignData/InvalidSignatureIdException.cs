using System.Runtime.Serialization;

namespace DustInTheWind.CryptoKeyManagement.Application.SignData;

[Serializable]
public class InvalidSignatureIdException : Exception
{
    public InvalidSignatureIdException(string signatureId)
        : base(string.Format("Invalid signature ID: {0}", signatureId))
    {
    }

    protected InvalidSignatureIdException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
