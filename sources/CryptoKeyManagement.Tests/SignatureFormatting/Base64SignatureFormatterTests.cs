using System.Text;
using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting;
using DustInTheWind.CryptoKeyManagement.Plugins.SignatureFormatting.Contracts;

namespace DustInTheWind.CryptoKeyManagement.Tests.SignatureFormatting;

/// <summary>
/// Example unit tests for Base64SignatureFormatter.
/// This demonstrates how the extracted formatter can be easily tested in isolation.
/// </summary>
public class Base64SignatureFormatterTests
{
    private readonly Base64SignatureFormatter formatter = new();
    private readonly KeyPair keyPair = new()
    {
        Id = Guid.NewGuid(),
        PrivateKey = new byte[] { 1, 2, 3 },
        PublicKey = new byte[] { 4, 5, 6 }
    };

    [Fact]
    public void FormatSignature_WithValidBytes_ReturnsBase64String()
    {
        // Arrange
        byte[] signature = Encoding.UTF8.GetBytes("test signature");
        string expected = Convert.ToBase64String(signature);

        // Act
        string result = formatter.FormatSignature(signature, keyPair);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FormatSignature_WithNullBytes_ReturnsEmptyString()
    {
        // Act
        string result = formatter.FormatSignature(null, keyPair);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FormatSignature_WithEmptyBytes_ReturnsEmptyString()
    {
        // Arrange
        byte[] emptySignature = Array.Empty<byte>();

        // Act
        string result = formatter.FormatSignature(emptySignature, keyPair);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}