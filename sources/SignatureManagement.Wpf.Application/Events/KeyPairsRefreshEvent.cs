using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

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