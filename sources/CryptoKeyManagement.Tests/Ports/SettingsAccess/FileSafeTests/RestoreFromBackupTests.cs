using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess.Helpers;

namespace DustInTheWind.CryptoKeyManagement.Tests.Ports.SettingsAccess.FileSafeTests;

public class RestoreFromBackupTests : IDisposable
{
    private readonly string testFilePath;
    private readonly string backupFilePath;

    public RestoreFromBackupTests()
    {
        testFilePath = Path.GetTempFileName();
        backupFilePath = testFilePath + ".bak";
    }

    public void Dispose()
    {
        if (File.Exists(testFilePath))
        {
            File.SetAttributes(testFilePath, FileAttributes.Normal);
            File.Delete(testFilePath);
        }

        if (File.Exists(backupFilePath))
        {
            File.SetAttributes(backupFilePath, FileAttributes.Normal);
            File.Delete(backupFilePath);
        }
    }

    [Fact]
    public void HavingExistingBackup_WhenRestoringFromBackup_ThenRestoresContent()
    {
        // Arrange
        string originalContent = "original content";
        string modifiedContent = "modified content";

        File.WriteAllText(testFilePath, originalContent);
        using FileSafe fileChangeSafe = new(testFilePath);

        // Simulate file corruption after backup was created
        File.WriteAllText(testFilePath, modifiedContent);

        // Act
        fileChangeSafe.RestoreFromBackup();

        // Assert
        string restoredContent = File.ReadAllText(testFilePath);
        Assert.Equal(originalContent, restoredContent);
    }

    [Fact]
    public void HavingNoFile_WhenCreatingInstanceAndRestoringFromBackup_ThenNothingHappens()
    {
        // Arrange
        if (File.Exists(testFilePath))
            File.Delete(testFilePath);

        using FileSafe fileChangeSafe = new(testFilePath);

        // Ensure no backup was created since file didn't exist
        Assert.False(File.Exists(backupFilePath));

        // Act
        fileChangeSafe.RestoreFromBackup(); // Should not throw and do nothing

        // Assert
        Assert.False(File.Exists(testFilePath)); // File should still not exist
    }

    [Fact]
    public void HavingNonExistentOriginalFile_WhenRestoringFromBackup_ThenCreatesFileFromBackup()
    {
        // Arrange
        string originalContent = "original content";
        File.WriteAllText(testFilePath, originalContent);
        using FileSafe fileChangeSafe = new(testFilePath);

        // Delete the original file to simulate it being lost
        File.Delete(testFilePath);

        // Act
        fileChangeSafe.RestoreFromBackup();

        // Assert
        Assert.True(File.Exists(testFilePath));
        string restoredContent = File.ReadAllText(testFilePath);
        Assert.Equal(originalContent, restoredContent);
    }

    [Fact]
    public void HavingBackupDeletedManually_WhenRestoringFromBackup_ThenNothingHappens()
    {
        // Arrange
        string originalContent = "original content";
        string modifiedContent = "modified content";
        File.WriteAllText(testFilePath, originalContent);
        using FileSafe fileChangeSafe = new(testFilePath);

        // Modify the file and manually delete the backup file
        File.WriteAllText(testFilePath, modifiedContent);
        File.Delete(backupFilePath);

        // Act
        fileChangeSafe.RestoreFromBackup(); // Should not throw and do nothing

        // Assert - file should remain with modified content since no backup exists
        string remainingContent = File.ReadAllText(testFilePath);
        Assert.Equal(modifiedContent, remainingContent);
    }
}