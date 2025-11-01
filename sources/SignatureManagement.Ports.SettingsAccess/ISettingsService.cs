using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.SettingsAccess;

public interface ISettingsService
{
    ThemeType ThemeType { get; set; }
}