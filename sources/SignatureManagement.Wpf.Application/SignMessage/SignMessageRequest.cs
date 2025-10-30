using AsyncMediator;

namespace DustInTheWind.SignatureManagement.Wpf.Application.SignMessage;

public class SignMessageRequest : ICommand
{
    public string Message { get; set; } = string.Empty;
}