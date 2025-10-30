using DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

public class SignatureKeySelectionChangedEvent
{
    public SignatureKeyDto SelectedKey { get; set; }
}
