using AsyncMediator;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.SignMessage;

public class SignMessageRequest : ICommand
{
    public string Message { get; set; } = string.Empty;
}