using System;
using System.IO;

namespace DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess.Helpers;

/// <summary>
/// Provides safe file modification operations with automatic backup and restore functionality.
/// Creates a backup immediately when instantiated and can restore from backup in case of errors.
/// </summary>
public class FileSafe : IDisposable
{
    private readonly string filePath;
    private readonly string backupFilePath;
    private bool disposed;

    public bool HasBackup => File.Exists(backupFilePath);

    public string BackupFilePath => backupFilePath;

    public string FilePath => filePath;

    public FileSafe(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath));

        this.filePath = filePath;
        backupFilePath = filePath + ".bak";

        CreateBackup();
    }

    private void CreateBackup()
    {
        try
        {
            if (File.Exists(filePath))
                File.Copy(filePath, backupFilePath, true);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to create backup of file '{filePath}'.", ex);
        }
    }

    public void RestoreFromBackup()
    {
        if (File.Exists(backupFilePath))
            File.Copy(backupFilePath, filePath, true);
    }

    public void Dispose()
    {
        if (!disposed)
        {
            RemoveBackup();
            disposed = true;
        }
    }

    private void RemoveBackup()
    {
        try
        {
            if (File.Exists(backupFilePath))
                File.Delete(backupFilePath);
        }
        catch (Exception)
        {
            // Ignore errors during cleanup
            // The backup file will remain but that's acceptable
        }
    }

    public static void ExecuteWithBackup(string filePath, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        using FileSafe fileChangeSafe = new(filePath);

        try
        {
            action?.Invoke();
        }
        catch (Exception)
        {
            fileChangeSafe.RestoreFromBackup();
            throw;
        }
    }
}