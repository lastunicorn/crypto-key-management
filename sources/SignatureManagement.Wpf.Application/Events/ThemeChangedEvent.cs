using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.Events;

public class ThemeChangedEvent
{
    public ThemeType ThemeType { get; set; }
}