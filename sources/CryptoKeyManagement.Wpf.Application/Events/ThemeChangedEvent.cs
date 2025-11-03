using DustInTheWind.CryptoKeyManagement.Domain;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;

public class ThemeChangedEvent
{
    public ThemeType ThemeType { get; set; }
}