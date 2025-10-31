using System.Windows;
using WpfApp = System.Windows.Application;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Services;

public class ThemeSelector
{
    private bool isDarkTheme = true;

    public bool IsDarkTheme
    {
        get => isDarkTheme;
        set
        {
            if (isDarkTheme != value)
            {
                isDarkTheme = value;
                OnThemeChanged();
            }
        }
    }

    public event EventHandler ThemeChanged;

    public void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        WpfApp app = WpfApp.Current;

        if (app?.Resources == null)
            return;

        app.Resources.MergedDictionaries.Clear();

        string themeUri = IsDarkTheme
            ? "pack://application:,,,/DustInTheWind.SignatureManagement.Wpf.Presentation;component/Styles/DarkTheme.xaml"
            : "pack://application:,,,/DustInTheWind.SignatureManagement.Wpf.Presentation;component/Styles/LightTheme.xaml";

        ResourceDictionary resourceDictionary = new()
        {
            Source = new Uri(themeUri)
        };
        app.Resources.MergedDictionaries.Add(resourceDictionary);
    }

    protected virtual void OnThemeChanged()
    {
        ThemeChanged?.Invoke(this, EventArgs.Empty);
    }
}
