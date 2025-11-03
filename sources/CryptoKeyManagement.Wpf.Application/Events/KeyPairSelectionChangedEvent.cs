using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

public class KeyPairSelectionChangedEvent
{
    public KeyPairDto SignatureKey { get; set; }
}
