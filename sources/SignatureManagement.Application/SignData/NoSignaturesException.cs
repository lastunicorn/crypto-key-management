using System.Runtime.Serialization;

namespace DustInTheWind.SignatureManagement.Application.SignData;

[Serializable]
internal class NoSignaturesException : Exception
{
    public NoSignaturesException()
        : base("No signatures available. Please create a signature first.")
    {
    }

    protected NoSignaturesException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
