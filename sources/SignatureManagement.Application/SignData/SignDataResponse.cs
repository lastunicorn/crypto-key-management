
namespace DustInTheWind.SignatureManagement.Application.SignData;

public class SignDataResponse
{
    public Guid SignatureId { get; internal set; }

    public string OriginalData { get; internal set; }

    public byte[] Signature { get; internal set; }
}