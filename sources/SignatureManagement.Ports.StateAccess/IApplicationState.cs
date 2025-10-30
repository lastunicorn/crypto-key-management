using System.ComponentModel;
using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.StateAccess;

public interface IApplicationState : INotifyPropertyChanged
{
    /// <summary>
    /// Gets or sets the currently selected signature key ID.
    /// </summary>
    SignatureKey CurrentSignatureKey { get; set; }
}