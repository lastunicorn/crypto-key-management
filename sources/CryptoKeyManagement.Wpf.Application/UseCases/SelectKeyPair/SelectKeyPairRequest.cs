using AsyncMediator;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SelectKeyPair;

public class SelectKeyPairRequest : ICommand
{
    public Guid? SignatureKeyId { get; set; }
}