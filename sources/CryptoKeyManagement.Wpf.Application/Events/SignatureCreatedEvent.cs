namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

public class SignatureCreatedEvent
{
    public string Message { get; set; }

    public byte[] Signature { get; set; }
}