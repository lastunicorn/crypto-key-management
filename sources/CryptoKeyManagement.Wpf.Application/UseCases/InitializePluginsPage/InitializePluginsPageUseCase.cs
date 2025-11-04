using System.Reflection;
using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializePluginsPage;

internal class InitializePluginsPageUseCase : IQuery<InitializePluginsPageRequest, InitializePluginsPageResponse>
{
    private readonly SignatureFormatterPool signatureFormatterPool;

    public InitializePluginsPageUseCase(SignatureFormatterPool signatureFormatterPool)
    {
        this.signatureFormatterPool = signatureFormatterPool ?? throw new ArgumentNullException(nameof(signatureFormatterPool));
    }

    public Task<InitializePluginsPageResponse> Query(InitializePluginsPageRequest criteria)
    {
        InitializePluginsPageResponse response = new()
        {
            Plugins = LoadPlugins()
        };

        return Task.FromResult(response);
    }

    private List<PluginDto> LoadPlugins()
    {
        return signatureFormatterPool
            .Select(x =>
            {
                Assembly assembly = x.GetType().Assembly;
                return new PluginDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    AssemblyName = assembly.GetName().Name,
                    Version = assembly.GetName().Version,
                    IsDefault = x == signatureFormatterPool.DefaultFormatter
                };
            })
            .OrderBy(x => x.AssemblyName)
            .ThenBy(x => x.Name)
            .ToList();
    }
}