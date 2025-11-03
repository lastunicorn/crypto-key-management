namespace DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;

/// <summary>
/// Represents user settings that are persisted to disk.
/// </summary>
public class JUserSettings
{
    /// <summary>
    /// Gets or sets whether the dark theme is enabled. Default is true.
    /// </summary>
    public JThemeType ThemeType { get; set; }
}
