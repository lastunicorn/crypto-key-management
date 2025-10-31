using System.ComponentModel;
using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.StateAccess;

public interface IApplicationState : INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets the currently selected signature key ID.
    /// </summary>
    SignatureKey CurrentSignatureKey { get; set; }

    /// <summary>
    /// Gets or sets the current message that was signed.
    /// </summary>
    string CurrentMessage { get; set; }

    /// <summary>
    /// Gets or sets the current signature generated for the message.
    /// </summary>
    string CurrentSignature { get; set; }
}