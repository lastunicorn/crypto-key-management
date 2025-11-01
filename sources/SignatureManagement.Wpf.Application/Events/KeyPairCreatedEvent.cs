using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

public class KeyPairCreatedEvent
{
    public KeyPairDto SignatureKey { get; set; }
}