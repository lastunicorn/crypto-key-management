namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

public class SignatureChangedEvent
{
    public string Message { get; set; }

    public string Signature { get; set; }
}