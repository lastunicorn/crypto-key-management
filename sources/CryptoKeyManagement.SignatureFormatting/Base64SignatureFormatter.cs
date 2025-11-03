namespace DustInTheWind.CryptoKeyManagement.SignatureFormatting;

/// <summary>
/// Formats signatures as Base64 strings for display in the presentation layer.
/// </summary>
public class Base64SignatureFormatter : ISignatureFormatter
{
    /// <summary>
    /// Formats a signature byte array into a Base64 string representation.
    /// </summary>
    /// <param name="signature">The signature bytes to format. Can be null or empty.</param>
    /// <returns>A Base64 string representation of the signature, or empty string if the input is null or empty.</returns>
    public string FormatSignature(byte[] signature)
    {
        return signature != null && signature.Length > 0
            ? Convert.ToBase64String(signature)
            : string.Empty;
    }
}