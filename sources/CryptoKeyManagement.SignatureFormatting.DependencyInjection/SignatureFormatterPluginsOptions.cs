using System.Reflection;

namespace DustInTheWind.CryptoKeyManagement.SignatureFormatting.DependencyInjection;

/// <summary>
/// Builder responsible for discovering all <see cref="ISignatureFormatter"/> implementations
/// across the currently loaded assemblies and constructing a configured <see cref="SignatureFormatterPool"/>.
/// </summary>
public class SignatureFormatterPluginsOptions
{
    private readonly HashSet<Assembly> assemblies = [];

    public SignatureFormatterPluginsOptions()
    {
        //assemblies.Add(Assembly.GetExecutingAssembly());
    }

    public SignatureFormatterPluginsOptions AddFromCurrentApplicationDomain()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
            _ = this.assemblies.Add(assembly);

        return this;
    }

    public SignatureFormatterPluginsOptions AddFromAssemblies(params Assembly[] assemblies)
    {
        foreach (Assembly assembly in assemblies)
            _ = this.assemblies.Add(assembly);

        return this;
    }

    /// <summary>
    /// Builds the <see cref="SignatureFormatterPool"/> by instantiating all discovered formatter types.
    /// </summary>
    /// <returns>A configured <see cref="SignatureFormatterPool"/> instance.</returns>
    internal IEnumerable<Type> GetSignatureFormatterTypes()
    {
        return assemblies
            .SelectMany(GetAssemblyTypesSafely)
            .Where(x => x != null)
            .Where(x => typeof(ISignatureFormatter).IsAssignableFrom(x))
            .Where(x => !x.IsInterface && !x.IsAbstract);
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
