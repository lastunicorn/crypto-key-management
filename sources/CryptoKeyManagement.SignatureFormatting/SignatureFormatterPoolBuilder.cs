using System.Reflection;

namespace DustInTheWind.CryptoKeyManagement.SignatureFormatting;

/// <summary>
/// Builder responsible for discovering all <see cref="ISignatureFormatter"/> implementations
/// across the currently loaded assemblies and constructing a configured <see cref="SignatureFormatterPool"/>.
/// </summary>
public class SignatureFormatterPoolBuilder
{
    private readonly HashSet<Assembly> assemblies = [];
    private IServiceProvider provider;

    public SignatureFormatterPoolBuilder(IServiceProvider provider)
    {
        this.provider = provider ?? throw new ArgumentNullException(nameof(provider));

        assemblies.Add(Assembly.GetExecutingAssembly());
    }

    public SignatureFormatterPoolBuilder AddFromCurrentApplicationDomain()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
            _ = this.assemblies.Add(assembly);

        return this;
    }

    public SignatureFormatterPoolBuilder AddFromAssemblies(params Assembly[] assemblies)
    {
        foreach (Assembly assembly in assemblies)
            _ = this.assemblies.Add(assembly);

        return this;
    }

    /// <summary>
    /// Builds the <see cref="SignatureFormatterPool"/> by instantiating all discovered formatter types.
    /// </summary>
    /// <returns>A configured <see cref="SignatureFormatterPool"/> instance.</returns>
    public SignatureFormatterPool Build()
    {
        IEnumerable<ISignatureFormatter> instances = assemblies
            .SelectMany(GetAssemblyTypesSafely)
            .Where(x => x != null)
            .Where(x => typeof(ISignatureFormatter).IsAssignableFrom(x))
            .Where(x => !x.IsInterface && !x.IsAbstract)
            .Select(x => (ISignatureFormatter)provider.GetService(x))
            .Where(x => x != null);

        return new SignatureFormatterPool(instances);
    }

    private static Type[] GetAssemblyTypesSafely(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types
                .Where(x => x != null)
                .ToArray();
        }
    }
}
