using System.Collections;
using System.Runtime.Serialization;
using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting.Contracts;

namespace DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting;

/// <summary>
/// Provides access to all registered <see cref="ISignatureFormatter"/> implementations and exposes a default formatter.
/// </summary>
public class SignatureFormatterPool : IEnumerable<ISignatureFormatter>
{
    private readonly List<ISignatureFormatter> formatters;

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

    public ISignatureFormatter Get(Guid id)
    {
        return formatters.FirstOrDefault(x => x.Id == id);
    }

    public bool ChooseDefaultFormatter(Func<ISignatureFormatter, bool> predicat)
    {
        ArgumentNullException.ThrowIfNull(predicat);

        ISignatureFormatter formatter = formatters.FirstOrDefault(predicat);

        if (formatter == null)
            return false;

        DefaultFormatter = formatter;

        return true;
    }

    public IEnumerator<ISignatureFormatter> GetEnumerator()
    {
        return formatters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
