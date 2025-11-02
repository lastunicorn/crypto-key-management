using System.Windows;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.WpfUserAccess;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

public partial class ConfirmationWindow : Window
{
    private readonly IThemeSelector themeSelector;

    public string Message { get; set; }

    public string KeyInfo { get; set; }

    public bool IsConfirmed { get; private set; }

    public ConfirmationWindow(IThemeSelector themeSelector)
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
        System.Windows.Application application = System.Windows.Application.Current;

        if (application?.Resources?.MergedDictionaries?.Count > 0)
        {
            var currentResourceDictionary = application.Resources.MergedDictionaries[0];

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

    private void YesButton_Click(object sender, RoutedEventArgs e)
    {
        IsConfirmed = true;
        DialogResult = true;
        Close();
    }

    private void NoButton_Click(object sender, RoutedEventArgs e)
    {
        IsConfirmed = false;
        DialogResult = false;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        IsConfirmed = false;
        DialogResult = false;
        Close();
    }
}