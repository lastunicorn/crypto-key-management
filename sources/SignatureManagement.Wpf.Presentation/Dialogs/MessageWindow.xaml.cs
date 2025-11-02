using System.Windows;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.WpfUserAccess;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Dialogs;

public partial class MessageWindow : Window
{
    private readonly IThemeSelector themeSelector;

    public string Message { get; set; }

    public MessageTypeEnum MessageType { get; set; }

    public MessageWindow(IThemeSelector themeSelector)
    {
        this.themeSelector = themeSelector ?? throw new ArgumentNullException(nameof(themeSelector));

        InitializeComponent();
        ApplyCurrentTheme();
        CenterDialogOnOwner();
    }

    private void ApplyCurrentTheme()
    {
        ThemeType currentTheme = GetCurrentTheme();
        themeSelector.ApplyTheme(currentTheme, this);
    }

    private ThemeType GetCurrentTheme()
    {
        // Check which theme is currently active by looking at the application resources
        if (System.Windows.Application.Current?.Resources?.MergedDictionaries?.Count > 0)
        {
            var currentResourceDictionary = System.Windows.Application.Current.Resources.MergedDictionaries[0];
            if (currentResourceDictionary.Source != null)
            {
                var sourceString = currentResourceDictionary.Source.ToString();
                if (sourceString.Contains("LightTheme.xaml"))
                    return ThemeType.Light;
                else if (sourceString.Contains("DarkTheme.xaml"))
                    return ThemeType.Dark;
            }
        }

        return ThemeType.Dark;
    }

    private void CenterDialogOnOwner()
    {
        if (System.Windows.Application.Current.MainWindow != null)
        {
            Owner = System.Windows.Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        else
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}