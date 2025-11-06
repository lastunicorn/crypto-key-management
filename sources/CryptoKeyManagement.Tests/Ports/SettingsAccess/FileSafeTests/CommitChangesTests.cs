using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess.Helpers;

namespace DustInTheWind.CryptoKeyManagement.Tests.Ports.SettingsAccess.FileSafeTests;

public class CommitChangesTests : IDisposable
{
    private readonly string testFilePath;
    private readonly string backupFilePath;

    public CommitChangesTests()
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
    public void HavingExistingBackup_WhenDisposing_ThenBackupFileIsDeleted()
    {
        // Arrange
        string content = "content";
        File.WriteAllText(testFilePath, content);
        FileSafe fileChangeSafe = new(testFilePath);

        Assert.True(File.Exists(backupFilePath)); // Verify backup was created by constructor

        // Act
        fileChangeSafe.Dispose();

        // Assert
        Assert.False(File.Exists(backupFilePath));
        Assert.False(fileChangeSafe.HasBackup);
    }

    [Fact]
    public void HavingNoFile_WhenCreatingInstanceAndDisposing_ThenNothingHappens()
    {
        // Arrange
        if (File.Exists(testFilePath))
            File.Delete(testFilePath);

        FileSafe fileChangeSafe = new(testFilePath);

        Assert.False(File.Exists(backupFilePath)); // Verify no backup was created

        // Act
        fileChangeSafe.Dispose();

        // Assert
        Assert.False(File.Exists(backupFilePath));
        Assert.False(fileChangeSafe.HasBackup);
    }

    [Fact]
    public void HavingBackupAfterDispose_WhenCreatingNewInstanceWithModifiedFile_ThenNewBackupIsCreated()
    {
        // Arrange
        string originalContent = "original";
        string modifiedContent = "modified";

        File.WriteAllText(testFilePath, originalContent);
        FileSafe firstInstance = new(testFilePath);

        // Dispose and modify file
        firstInstance.Dispose();
        File.WriteAllText(testFilePath, modifiedContent);

        // Act - create new instance with modified file
        using FileSafe secondInstance = new(testFilePath);

        // Assert
        Assert.True(File.Exists(backupFilePath));
        Assert.True(secondInstance.HasBackup);

        string backupContent = File.ReadAllText(backupFilePath);
        Assert.Equal(modifiedContent, backupContent); // Should contain the modified content
    }

    [Fact]
    public void HavingMultipleDisposes_WhenDisposingMultipleTimes_ThenSubsequentDisposesDoNothing()
    {
        // Arrange
        string content = "content";
        File.WriteAllText(testFilePath, content);
        FileSafe fileChangeSafe = new(testFilePath);

        // Act - dispose multiple times
        fileChangeSafe.Dispose();
        fileChangeSafe.Dispose(); // Second dispose should do nothing

        // Assert
        Assert.False(File.Exists(backupFilePath));
        Assert.False(fileChangeSafe.HasBackup);
    }

    [Fact]
    public void HavingUsingStatement_WhenExitingUsingBlock_ThenBackupIsAutomaticallyDeleted()
    {
        // Arrange
        string content = "content";
        File.WriteAllText(testFilePath, content);

        // Act
        using (FileSafe fileChangeSafe = new(testFilePath))
        {
            Assert.True(File.Exists(backupFilePath)); // Verify backup exists inside using block
            Assert.True(fileChangeSafe.HasBackup);
        }

        // Assert - backup should be cleaned up after using block
        Assert.False(File.Exists(backupFilePath));
    }
}