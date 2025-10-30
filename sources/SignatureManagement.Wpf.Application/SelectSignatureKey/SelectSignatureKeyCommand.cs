using AsyncMediator;

namespace DustInTheWind.SignatureManagement.Wpf.Application.SelectSignatureKey;

public class SelectSignatureKeyCommand : ICommand
{
    public Guid? SignatureKeyId { get; set; }
}