using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;

public partial class PluginView : UserControl
{
    public static readonly DependencyProperty CopyToClipboardCommandProperty = DependencyProperty.Register(
        nameof(CopyToClipboardCommand),
        typeof(ICommand),
        typeof(PluginView),
        new PropertyMetadata(null));

    public ICommand CopyToClipboardCommand
    {
        get => (ICommand)GetValue(CopyToClipboardCommandProperty);
        set => SetValue(CopyToClipboardCommandProperty, value);
    }

    public PluginView()
    {
        InitializeComponent();
    }
}