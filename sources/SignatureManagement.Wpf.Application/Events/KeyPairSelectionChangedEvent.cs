using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

public class KeyPairSelectionChangedEvent
{
    public KeyPairDto SignatureKey { get; set; }
}
