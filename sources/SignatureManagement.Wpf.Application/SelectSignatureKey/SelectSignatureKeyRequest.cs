using AsyncMediator;

namespace DustInTheWind.SignatureManagement.Wpf.Application.SelectSignatureKey;

public class SelectSignatureKeyRequest : ICommand
{
    public Guid? SignatureKeyId { get; set; }
}