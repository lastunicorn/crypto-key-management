using AsyncMediator;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SignMessage;

public class SignMessageRequest : ICommand
{
    public string Message { get; set; } = string.Empty;
}