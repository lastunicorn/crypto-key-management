using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting.Contracts;

namespace DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting;

/// <summary>
/// Formats signatures as Base64 strings for display in the presentation layer.
/// </summary>
public class Base64SignatureFormatter : ISignatureFormatter
{
    public Guid Id => new("CCF75B79-7EA4-46B8-AF31-FBFE88BD0911");

    public string Name => "Base64";

    /// <summary>
    /// Formats a signature byte array into a Base64 string representation.
    /// </summary>
    /// <param name="signature">The signature bytes to format. Can be null or empty.</param>
    /// <returns>A Base64 string representation of the signature, or empty string if the input is null or empty.</returns>
    public string FormatSignature(byte[] signature, KeyPair keyPair)
    {
        return signature != null && signature.Length > 0
            ? Convert.ToBase64String(signature)
            : string.Empty;
    }
}