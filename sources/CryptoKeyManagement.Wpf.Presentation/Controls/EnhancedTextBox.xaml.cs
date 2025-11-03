using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Controls;

/// <summary>
/// A custom control that combines a read-only TextBox with a Copy button for clipboard functionality.
/// </summary>
public partial class EnhancedTextBox : UserControl
{
    /// <summary>
    /// Dependency property for the Text that will be displayed and copied.
    /// </summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(EnhancedTextBox),
        new PropertyMetadata(string.Empty));

    /// <summary>
    /// Dependency property for the VerticalScrollBarVisibility of the internal TextBox.
    /// </summary>
    public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
        DependencyProperty.Register(
        nameof(VerticalScrollBarVisibility),
        typeof(ScrollBarVisibility),
        typeof(EnhancedTextBox),
        new PropertyMetadata(ScrollBarVisibility.Disabled));

    /// <summary>
    /// Dependency property for the TextWrapping of the internal TextBox.
    /// </summary>
    public static readonly DependencyProperty TextWrappingProperty =
        DependencyProperty.Register(
        nameof(TextWrapping),
        typeof(TextWrapping),
        typeof(EnhancedTextBox),
        new PropertyMetadata(TextWrapping.NoWrap));

    /// <summary>
    /// Gets or sets the text content of the control.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the vertical scroll bar visibility for the internal TextBox.
    /// </summary>
    public ScrollBarVisibility VerticalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
        set => SetValue(VerticalScrollBarVisibilityProperty, value);
    }

    /// <summary>
    /// Gets or sets the text wrapping behavior for the internal TextBox.
    /// </summary>
    public TextWrapping TextWrapping
    {
        get => (TextWrapping)GetValue(TextWrappingProperty);
        set => SetValue(TextWrappingProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the EnhancedTextBox class.
    /// </summary>
    public EnhancedTextBox()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event for copying the text to clipboard.
    /// </summary>
    private void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Text))
            Clipboard.SetText(Text);
    }
}