using System.Reflection;
using DustInTheWind.CryptoKeyManagement.SignatureFormatting.Contracts;

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
        assemblies.Add(Assembly.GetExecutingAssembly());
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
    /// Loads all .dll assemblies from the specified directory (optionally including subdirectories) and registers them for plugin discovery.
    /// Non-.NET binaries or load failures are silently ignored.
    /// </summary>
    /// <param name="directoryPath">The absolute path of the directory containing plugin assemblies.</param>
    /// <param name="includeSubdirectories">Indicates whether to search through all subdirectories recursively.</param>
    /// <returns>The current <see cref="SignatureFormatterPluginsOptions"/> instance for fluent configuration.</returns>
    public SignatureFormatterPluginsOptions AddFromDirectory(string directoryPath, bool includeSubdirectories = false)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            return this;

        if (!Directory.Exists(directoryPath))
            return this;

        SearchOption searchOption = includeSubdirectories
            ? SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;

        string[] dllFiles = Directory.GetFiles(directoryPath, "*.dll", searchOption);

        foreach (string dllFile in dllFiles)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllFile);
                _ = assemblies.Add(assembly);
            }
            catch (BadImageFormatException)
            {
                // Not a valid .NET assembly; ignore.
            }
            catch (FileLoadException)
            {
                // Assembly could not be loaded (already loaded or locked); ignore.
            }
        }

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
