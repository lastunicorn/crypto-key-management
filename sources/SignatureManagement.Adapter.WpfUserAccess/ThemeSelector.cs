using System.Windows;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.WpfUserAccess;
using WpfApp = System.Windows.Application;

namespace DustInTheWind.SignatureManagement.Adapter.WpfUserAccess;

public class ThemeSelector : IThemeSelector
{
    public void ApplyTheme(ThemeType themeType)
    {
        WpfApp app = WpfApp.Current;

        if (app?.Resources == null)
            return;

        ResourceDictionary resourceDictionary = new()
        {
            Source = GenerateThemeUri(themeType)
        };

        app.Resources.MergedDictionaries.Clear();
        app.Resources.MergedDictionaries.Add(resourceDictionary);
    }

    public void ApplyTheme(ThemeType themeType, object target)
    {
        if (target is not FrameworkElement frameworkElement)
            throw new ArgumentException("Target must be a Window.", nameof(target));

        ResourceDictionary resourceDictionary = new()
        {
            Source = GenerateThemeUri(themeType)
        };

        frameworkElement.Resources.MergedDictionaries.Clear();
        frameworkElement.Resources.MergedDictionaries.Add(resourceDictionary);
    }

    private static Uri GenerateThemeUri(ThemeType themeType)
    {
        switch (themeType)
        {
            case ThemeType.Light:
                return new Uri("pack://application:,,,/DustInTheWind.SignatureManagement.Wpf.Presentation;component/Styles/LightTheme.xaml");

            case ThemeType.Dark:
                return new Uri("pack://application:,,,/DustInTheWind.SignatureManagement.Wpf.Presentation;component/Styles/DarkTheme.xaml");

            default:
                throw new ArgumentOutOfRangeException(nameof(themeType), themeType, null);
        }
    }
}
