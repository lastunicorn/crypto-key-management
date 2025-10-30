using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

public class SignatureKeySelectionChangedEvent
{
    public SignatureKeyDto SignatureKey { get; set; }
}
