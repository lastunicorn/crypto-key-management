using AsyncMediator;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SetDefaultPlugin;

public class SetDefaultPluginRequest : ICommand
{
    public Guid PluginId { get; set; }
}