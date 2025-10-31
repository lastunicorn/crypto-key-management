namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

public class SignatureCreatedEvent
{
    public string Message { get; set; }

    public string Signature { get; set; }
}