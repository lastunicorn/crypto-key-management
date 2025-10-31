using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;

/// <summary>
/// User control that displays the key information including Key ID, Private Key, and Public Key.
/// </summary>
public partial class KeyInfoControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the KeyInfoControl class.
    /// </summary>
    public KeyInfoControl()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event for copying the Key ID to clipboard.
    /// </summary>
    private void CopyKeyId_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(KeyIdTextBox.Text))
        {
            Clipboard.SetText(KeyIdTextBox.Text);
        }
    }

    /// <summary>
    /// Handles the click event for copying the Private Key to clipboard.
    /// </summary>
    private void CopyPrivateKey_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(PrivateKeyTextBox.Text))
        {
            Clipboard.SetText(PrivateKeyTextBox.Text);
        }
    }

    /// <summary>
    /// Handles the click event for copying the Public Key to clipboard.
    /// </summary>
    private void CopyPublicKey_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(PublicKeyTextBox.Text))
        {
            Clipboard.SetText(PublicKeyTextBox.Text);
        }
    }
}