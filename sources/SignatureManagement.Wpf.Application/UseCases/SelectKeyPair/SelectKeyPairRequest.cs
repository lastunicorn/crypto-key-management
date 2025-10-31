using AsyncMediator;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SelectKeyPair;

public class SelectKeyPairRequest : ICommand
{
    public Guid? SignatureKeyId { get; set; }
}