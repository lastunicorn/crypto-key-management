using System.Runtime.Serialization;

namespace DustInTheWind.SignatureManagement.Application.SignData;

[Serializable]
internal class NoDataToSignException : Exception
{
    public NoDataToSignException()
        : base("No data provided.")
    {
    }

    protected NoDataToSignException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}