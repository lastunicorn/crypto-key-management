using DustInTheWind.CryptoKeyManagement.Ports.SettingsAccess.Helpers;

namespace DustInTheWind.CryptoKeyManagement.Tests.Ports.SettingsAccess.FileSafeTests;

public class ConstructorTests : IDisposable
{
    private readonly string testFilePath;
    private readonly string backupFilePath;

    public ConstructorTests()
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
    public void HavingNullFilePath_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new FileSafe(null);
        });

        Assert.Equal("filePath", exception.ParamName);
    }

    [Fact]
    public void HavingEmptyFilePath_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new FileSafe(string.Empty);
        });

        Assert.Equal("filePath", exception.ParamName);
    }

    [Fact]
    public void HavingWhitespaceFilePath_WhenCreatingInstance_ThenThrowsArgumentNullException()
    {
        // Act & Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = new FileSafe(" ");
        });

        Assert.Equal("filePath", exception.ParamName);
    }

    [Fact]
    public void HavingValidFilePath_WhenCreatingInstance_ThenSetsFilePathProperty()
    {
        // Arrange
        string expectedFilePath = "test.txt";

        // Act
        using FileSafe fileChangeSafe = new(expectedFilePath);

        // Assert
        Assert.Equal(expectedFilePath, fileChangeSafe.FilePath);
    }

    [Fact]
    public void HavingValidFilePath_WhenCreatingInstance_ThenSetsBackupFilePathProperty()
    {
        // Arrange
        string filePath = "test.txt";
        string expectedBackupFilePath = "test.txt.bak";

        // Act
        using FileSafe fileChangeSafe = new(filePath);

        // Assert
        Assert.Equal(expectedBackupFilePath, fileChangeSafe.BackupFilePath);
    }

    [Fact]
    public void HavingNonExistentFile_WhenCreatingInstance_ThenHasBackupIsFalse()
    {
        // Arrange
        string nonExistentFilePath = "non-existent-file.txt";

        // Ensure file doesn't exist
        if (File.Exists(nonExistentFilePath))
            File.Delete(nonExistentFilePath);

        // Act
        using FileSafe fileChangeSafe = new(nonExistentFilePath);

        // Assert
        Assert.False(fileChangeSafe.HasBackup);
    }

    [Fact]
    public void HavingExistingFile_WhenCreatingInstance_ThenBackupIsCreatedAutomatically()
    {
        // Arrange
        string content = "test content";
        File.WriteAllText(testFilePath, content);

        // Act
        using FileSafe fileChangeSafe = new(testFilePath);

        // Assert
        Assert.True(fileChangeSafe.HasBackup);
        Assert.True(File.Exists(backupFilePath));
        string backupContent = File.ReadAllText(backupFilePath);
        Assert.Equal(content, backupContent);
    }

    [Fact]
    public void HavingExistingFileAndFailedBackupCreation_WhenCreatingInstance_ThenThrowsIOException()
    {
        // Arrange
        File.WriteAllText(testFilePath, "content");

        // Create a directory with the same name as backup file to cause backup creation to fail
        string conflictingDirectoryPath = backupFilePath;
        _ = Directory.CreateDirectory(conflictingDirectoryPath);

        try
        {
            // Act & Assert
            IOException exception = Assert.Throws<IOException>(() =>
            {
                _ = new FileSafe(testFilePath);
            });

            Assert.Contains("Failed to create backup", exception.Message);
        }
        finally
        {
            // Clean up
            if (Directory.Exists(conflictingDirectoryPath))
                Directory.Delete(conflictingDirectoryPath);
        }
    }
}