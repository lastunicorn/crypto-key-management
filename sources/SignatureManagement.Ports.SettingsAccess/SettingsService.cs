using System.Text.Json;

namespace DustInTheWind.SignatureManagement.Ports.SettingsAccess;

/// <summary>
/// Service for managing application settings, including theme preferences.
/// Settings are automatically saved to a JSON file in the user's AppData folder.
/// </summary>
public class SettingsService : ISettingsService
{
    private readonly string settingsFilePath;
    private UserSettings settings;

    /// <summary>
    /// Initializes a new instance of the SettingsService and loads existing settings.
    /// </summary>
    public SettingsService()
    {
        string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appDataFolder, "SignatureManagement");
        Directory.CreateDirectory(appFolder);
        settingsFilePath = Path.Combine(appFolder, "settings.json");

        LoadSettings();
    }

    /// <summary>
    /// Gets or sets whether the dark theme is enabled. 
    /// Changes are automatically persisted to the settings file.
    /// </summary>
    public bool IsDarkTheme
    {
        get => settings.IsDarkTheme;
        set
        {
            if (settings.IsDarkTheme != value)
            {
                settings.IsDarkTheme = value;
                SaveSettings();
            }
        }
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(settingsFilePath))
            {
                string json = File.ReadAllText(settingsFilePath);
                settings = JsonSerializer.Deserialize<UserSettings>(json) ?? new UserSettings();
            }
            else
                settings = new UserSettings();
        }
        catch
        {
            // If there's any error loading settings, use defaults
            settings = new UserSettings();
            // You might want to log this exception in a real application
        }
    }

    private void SaveSettings()
    {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(settingsFilePath, json);
    }
}
