using System.Windows;
using DustInTheWind.SignatureManagement.Ports.SettingsAccess;
using WpfApp = System.Windows.Application;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Services;

public class ThemeSelector
{
    private readonly ISettingsService settingsService;

    public ThemeSelector(ISettingsService settingsService)
    {
        this.settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

        ApplyTheme();
    }

    public bool IsDarkTheme
    {
        get => settingsService.IsDarkTheme;
        set
        {
            if (settingsService.IsDarkTheme != value)
            {
                settingsService.IsDarkTheme = value;
                ApplyTheme();
                OnThemeChanged();
            }
        }
    }

    public event EventHandler ThemeChanged;

    public void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
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
