using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

public class KeyPairCreatedEvent
{
    public KeyPairDto SignatureKey { get; set; }
}