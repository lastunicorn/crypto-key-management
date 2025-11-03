using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

public class KeyPairSelectionChangedEvent
{
    public KeyPairDto SignatureKey { get; set; }
}
