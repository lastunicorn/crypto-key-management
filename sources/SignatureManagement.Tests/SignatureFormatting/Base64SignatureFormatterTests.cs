using System.Text;
using DustInTheWind.SignatureManagement.SignatureFormatting;

namespace DustInTheWind.SignatureManagement.Tests.SignatureFormatting;

/// <summary>
/// Example unit tests for Base64SignatureFormatter.
/// This demonstrates how the extracted formatter can be easily tested in isolation.
/// </summary>
public class Base64SignatureFormatterTests
{
    private readonly Base64SignatureFormatter formatter = new();

    [Fact]
    public void FormatSignature_WithValidBytes_ReturnsBase64String()
    {
        // Arrange
        byte[] signature = Encoding.UTF8.GetBytes("test signature");
        string expected = Convert.ToBase64String(signature);

        // Act
        string result = formatter.FormatSignature(signature);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FormatSignature_WithNullBytes_ReturnsEmptyString()
    {
        // Act
        string result = formatter.FormatSignature(null);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FormatSignature_WithEmptyBytes_ReturnsEmptyString()
    {
        // Arrange
        byte[] emptySignature = Array.Empty<byte>();

        // Act
        string result = formatter.FormatSignature(emptySignature);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}