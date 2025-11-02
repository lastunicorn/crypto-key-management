using AsyncMediator;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.DeleteKeyPair;

public class DeleteKeyPairRequest : ICommand
{
    public Guid KeyPairId { get; set; }
}