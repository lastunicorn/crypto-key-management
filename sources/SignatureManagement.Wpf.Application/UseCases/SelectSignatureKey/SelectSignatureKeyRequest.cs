using AsyncMediator;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SelectSignatureKey;

public class SelectSignatureKeyRequest : ICommand
{
    public Guid? SignatureKeyId { get; set; }
}