namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

/// <summary>
/// View model representing a signature key in the keys selector.
/// </summary>
public class SignatureKeyViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the signature key.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the formatted creation date text.
    /// </summary>
    public string CreatedDateText { get; set; }

    /// <summary>
    /// Gets or sets the private key encoded as Base64 string.
    /// </summary>
    public string PrivateKeyBase64 { get; set; }

    /// <summary>
    /// Gets or sets the public key encoded as Base64 string.
    /// </summary>
    public string PublicKeyBase64 { get; set; }
}