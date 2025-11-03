using System.Windows.Controls;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPanel;

/// <summary>
/// User control that provides UI for message input and signature generation.
/// Contains a text box for message input, a button to sign the message, and a read-only text box to display the generated signature.
/// </summary>
public partial class SigningPanelControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the SigningPanelControl class.
    /// </summary>
    public SigningPanelControl()
    {
        InitializeComponent();
    }
}