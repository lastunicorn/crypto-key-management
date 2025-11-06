using System;
using System.IO;
using System.Text.Json;
using DustInTheWind.CryptoKeyManagement.Domain;
using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess.Helpers;

namespace DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess;

/// <summary>
/// Service for managing application settings, including theme preferences.
/// Settings are automatically saved to a JSON file in the user's AppData folder.
/// </summary>
public class SettingsService : ISettingsService
{
    private readonly string settingsFilePath;
    private JUserSettings userSettings;

    /// <summary>
    /// Initializes a new instance of the SettingsService and loads existing settings.
    /// </summary>
    public SettingsService()
    {
        string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appDataFolder, "Crypto Key Management");
        _ = Directory.CreateDirectory(appFolder);
        settingsFilePath = Path.Combine(appFolder, "settings.json");

        LoadSettings();
    }

    /// <summary>
    /// Gets or sets whether the dark theme is enabled. 
    /// Changes are automatically persisted to the settings file.
    /// </summary>
    public ThemeType ThemeType
    {
        get => ToEntity(userSettings.ThemeType);
        set
        {
            JThemeType newValue = ToJEntity(value);

            if (userSettings.ThemeType != newValue)
            {
                userSettings.ThemeType = newValue;
                SaveSettings();
            }
        }
    }

    public Guid? SignatureFormatterId
    {
        get => userSettings.SignatureFormatterId;
        set
        {
            if (userSettings.SignatureFormatterId != value)
            {
                userSettings.SignatureFormatterId = value;
                SaveSettings();
            }
        }
    }

    private static JThemeType ToJEntity(ThemeType value)
    {
        return value switch
        {
            ThemeType.Dark => JThemeType.Dark,
            ThemeType.Light => JThemeType.Light,
            _ => JThemeType.Light,
        };
    }

    private ThemeType ToEntity(JThemeType jThemeType)
    {
        return jThemeType switch
        {
            JThemeType.None => ThemeType.Light,
            JThemeType.Dark => ThemeType.Dark,
            JThemeType.Light => ThemeType.Light,
            _ => ThemeType.Light,
        };
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(settingsFilePath))
            {
                string json = File.ReadAllText(settingsFilePath);
                userSettings = JsonSerializer.Deserialize<JUserSettings>(json) ?? new JUserSettings();
            }
            else
                userSettings = new JUserSettings();
        }
        catch
        {
            userSettings = new JUserSettings();
            // You might want to log this exception in a real application
        }
    }

    private void SaveSettings()
    {
        FileSafe.ExecuteWithBackup(settingsFilePath, () =>
        {
            string json = JsonSerializer.Serialize(userSettings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(settingsFilePath, json);
        });
    }
}
