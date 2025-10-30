namespace DustInTheWind.SignatureManagement.Ports.StateAccess;

public interface IApplicationState
{
    /// <summary>
    /// Gets or sets the currently selected signature key ID.
    /// </summary>
    Guid? SelectedSignatureKeyId { get; set; }

    /// <summary>
    /// Event raised when the selected signature key changes.
    /// </summary>
    event EventHandler<Guid?> SelectedSignatureKeyChanged;
}