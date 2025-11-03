using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

/// <summary>
/// Event raised when the signature keys list has been refreshed.
/// </summary>
public class KeyPairsRefreshEvent
{
    /// <summary>
    /// Gets or sets the refreshed list of signature keys.
    /// </summary>
    public List<KeyPairDto> SignatureKeys { get; set; } = new();
}