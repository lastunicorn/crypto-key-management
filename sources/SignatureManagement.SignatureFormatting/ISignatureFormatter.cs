namespace DustInTheWind.SignatureManagement.SignatureFormatting;

/// <summary>
/// Interface for formatting signatures for display purposes.
/// </summary>
public interface ISignatureFormatter
{
    /// <summary>
    /// Formats a signature byte array into a string representation for display.
    /// </summary>
    /// <param name="signature">The signature bytes to format.</param>
    /// <returns>A formatted string representation of the signature.</returns>
    string FormatSignature(byte[] signature);
}