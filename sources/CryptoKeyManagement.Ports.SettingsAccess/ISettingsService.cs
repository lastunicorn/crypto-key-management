using DustInTheWind.CryptoKeyManagement.Domain;

namespace DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;

public interface ISettingsService
{
    ThemeType ThemeType { get; set; }

    Guid? SignatureFormatterId { get; set; }
}