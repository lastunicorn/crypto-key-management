using AsyncMediator;

namespace DustInTheWind.SignatureManagement.Wpf.Application.SignMessage;

public class SignMessageCommand : ICommand
{
    public string Message { get; set; } = string.Empty;
    public Guid? SignatureKeyId { get; set; }
}