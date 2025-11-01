namespace DustInTheWind.SignatureManagement.Ports.SettingsAccess;

/// <summary>
/// Represents user settings that are persisted to disk.
/// </summary>
public class UserSettings
{
    /// <summary>
    /// Gets or sets whether the dark theme is enabled. Default is true.
    /// </summary>
    public bool IsDarkTheme { get; set; } = true;
}