using System.ComponentModel;
using DustInTheWind.CryptoKeyManagement.Domain;

namespace DustInTheWind.CryptoKeyManagement.Ports.StateAccess;

public interface IApplicationState : INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets the currently selected signature key ID.
    /// </summary>
    KeyPair CurrentSignatureKey { get; set; }

    /// <summary>
    /// Gets or sets the current message that was signed.
    /// </summary>
    string CurrentMessage { get; set; }

    /// <summary>
    /// Gets or sets the current signature generated for the message.
    /// </summary>
    byte[] CurrentSignature { get; set; }
}