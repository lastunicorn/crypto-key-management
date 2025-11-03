using DustInTheWind.CryptoKeyManagement.Domain;

namespace DustInTheWind.CryptoKeyManagement.Ports.WpfUserAccess;

public interface IThemeSelector
{
    void ApplyTheme(ThemeType themeType);
    
    void ApplyTheme(ThemeType themeType, object target);
}