namespace DustInTheWind.CryptoKeyManagement.SignatureFormatting.Contracts;

/// <summary>
/// Interface for formatting signatures for display purposes.
/// </summary>
public interface ISignatureFormatter
{
    public Guid Id { get; }

    public string Name { get; }

    /// <summary>
    /// Formats a signature byte array into a string representation for display.
    /// </summary>
    /// <param name="signature">The signature bytes to format.</param>
    /// <returns>A formatted string representation of the signature.</returns>
    string FormatSignature(byte[] signature);
}