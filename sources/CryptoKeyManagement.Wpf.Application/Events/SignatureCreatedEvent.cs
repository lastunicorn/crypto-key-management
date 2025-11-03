using DustInTheWind.CryptoKeyManagement.Domain;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

public class SignatureCreatedEvent
{
    public string Message { get; set; }

    public byte[] Signature { get; set; }
    public KeyPair KeyPair { get; internal set; }
}