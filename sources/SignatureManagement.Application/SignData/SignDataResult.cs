
namespace DustInTheWind.SignatureManagement.Application.SignData;

public class SignDataResult
{
    public Guid SignatureId { get; internal set; }
    public string OriginalData { get; internal set; }
    public string SignedDataBase64 { get; internal set; }
}