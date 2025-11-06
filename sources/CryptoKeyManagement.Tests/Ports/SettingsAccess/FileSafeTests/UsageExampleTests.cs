using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess.Helpers;

namespace DustInTheWind.CryptoKeyManagement.Tests.Ports.SettingsAccess.FileSafeTests;

public class UsageExampleTests : IDisposable
{
    private readonly string testFilePath;
    private readonly string backupFilePath;

    public UsageExampleTests()
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
    public void ExampleUsage_SuccessfulWrite_ThenBackupIsCleanedUpAutomatically()
    {
        // Arrange
        string originalContent = "original content";
        string newContent = "new content";
        File.WriteAllText(testFilePath, originalContent);

        // Act - using statement ensures automatic cleanup
        using (FileSafe fileChangeSafe = new(testFilePath))
        {
            // Verify backup was created
            Assert.True(fileChangeSafe.HasBackup);

            // Perform file operations directly
            File.WriteAllText(testFilePath, newContent);

        } // Dispose is called here, which cleans up the backup

        // Assert
        Assert.False(File.Exists(backupFilePath)); // Backup cleaned up
        string finalContent = File.ReadAllText(testFilePath);
        Assert.Equal(newContent, finalContent);
    }

    [Fact]
    public void ExampleUsage_ManualRestoreFromBackup_WhenNeeded()
    {
        // Arrange
        string originalContent = "original content";
        string corruptedContent = "corrupted content";
        File.WriteAllText(testFilePath, originalContent);

        using FileSafe fileChangeSafe = new(testFilePath);

        // Simulate file getting corrupted after backup was created
        File.WriteAllText(testFilePath, corruptedContent);

        // Manually restore from backup when needed
        fileChangeSafe.RestoreFromBackup();

        // Assert - file should be restored to original content
        string restoredContent = File.ReadAllText(testFilePath);
        Assert.Equal(originalContent, restoredContent);

        // Backup will be automatically cleaned up when disposed
    }
}