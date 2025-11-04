using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting;
using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddSignatureFormattingPlugins(this IServiceCollection services, Action<SignatureFormatterPluginsOptions> configure)
    {
        SignatureFormatterPluginsOptions options = new();
        configure?.Invoke(options);

        IEnumerable<Type> formatterTypes = options.GetSignatureFormatterTypes();

        foreach (Type formatterType in formatterTypes)
            services.AddTransient(typeof(ISignatureFormatter), formatterType);

        services.AddSingleton(provider =>
        {
            IEnumerable<ISignatureFormatter> instances = provider.GetServices<ISignatureFormatter>();
            return new SignatureFormatterPool(instances);
        });
    }
}
