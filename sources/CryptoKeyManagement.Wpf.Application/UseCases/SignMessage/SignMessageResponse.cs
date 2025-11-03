namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SignMessage;

public class SignMessageResponse
{
    public byte[] Signature { get; set; } = Array.Empty<byte>();
    public string Message { get; set; } = string.Empty;
}