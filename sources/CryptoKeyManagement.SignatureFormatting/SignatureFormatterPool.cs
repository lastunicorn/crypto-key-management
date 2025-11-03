namespace DustInTheWind.CryptoKeyManagement.SignatureFormatting;

/// <summary>
/// Provides access to all registered <see cref="ISignatureFormatter"/> implementations and exposes a default formatter.
/// </summary>
public class SignatureFormatterPool
{
    private readonly List<ISignatureFormatter> allFormatters;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureFormatterPool"/> class.
    /// The first formatter in the provided enumerable becomes the default one.
    /// </summary>
    /// <param name="formatters">All registered <see cref="ISignatureFormatter"/> implementations.</param>
    public SignatureFormatterPool(IEnumerable<ISignatureFormatter> formatters)
    {
        allFormatters = formatters != null 
            ? formatters.ToList()
            : [];

        DefaultFormatter = allFormatters.FirstOrDefault();
    }

    /// <summary>
    /// Gets the default formatter (the first registered one) or null if none exist.
    /// </summary>
    public ISignatureFormatter DefaultFormatter { get; }

    /// <summary>
    /// Returns all registered formatters in registration order.
    /// </summary>
    public IReadOnlyCollection<ISignatureFormatter> AllFormatters => allFormatters;

    /// <summary>
    /// Returns the default formatter. Convenience wrapper around <see cref="DefaultFormatter"/>.
    /// </summary>
    public ISignatureFormatter GetDefaultFormatter()
    {
        return DefaultFormatter;
    }
}
