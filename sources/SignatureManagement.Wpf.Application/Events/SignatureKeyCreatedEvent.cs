using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

public class SignatureKeyCreatedEvent
{
    public SignatureKeyDto SignatureKey { get; set; }
}