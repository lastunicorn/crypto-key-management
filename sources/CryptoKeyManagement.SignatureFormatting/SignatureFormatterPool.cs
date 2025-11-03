using DustInTheWind.CryptoKeyManagement.SignatureFormatting.Contracts;

namespace DustInTheWind.CryptoKeyManagement.SignatureFormatting;

/// <summary>
/// Provides access to all registered <see cref="ISignatureFormatter"/> implementations and exposes a default formatter.
/// </summary>
public class SignatureFormatterPool
{
    private readonly List<ISignatureFormatter> formatters;

    /// <summary>
    /// Returns all registered formatters in registration order.
    /// </summary>
    public IReadOnlyCollection<ISignatureFormatter> Formatters => formatters;

    /// <summary>
    /// Gets the default formatter (the first registered one) or null if none exist.
    /// </summary>
    public ISignatureFormatter DefaultFormatter { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureFormatterPool"/> class.
    /// The first formatter in the provided enumerable becomes the default one.
    /// </summary>
    /// <param name="formatters">All registered <see cref="ISignatureFormatter"/> implementations.</param>
    public SignatureFormatterPool(IEnumerable<ISignatureFormatter> formatters)
    {
        this.formatters = formatters != null 
            ? formatters.ToList()
            : [];

        DefaultFormatter = this.formatters.FirstOrDefault();
    }
}
