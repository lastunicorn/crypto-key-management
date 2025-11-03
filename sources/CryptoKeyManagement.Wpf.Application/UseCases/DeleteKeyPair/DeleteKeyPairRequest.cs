using AsyncMediator;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.DeleteKeyPair;

public class DeleteKeyPairRequest : ICommand
{
    public Guid KeyPairId { get; set; }
}