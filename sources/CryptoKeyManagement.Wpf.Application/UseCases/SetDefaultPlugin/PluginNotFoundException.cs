
namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.SetDefaultPlugin;

internal class PluginNotFoundException : Exception
{
    public PluginNotFoundException(Guid pluginId)
        : base($"Plugin with ID {pluginId} not found.")
    {
    }
}