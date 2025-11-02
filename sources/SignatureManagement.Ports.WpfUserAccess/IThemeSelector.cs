using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.WpfUserAccess;

public interface IThemeSelector
{
    void ApplyTheme(ThemeType themeType);
    
    void ApplyTheme(ThemeType themeType, object target);
}