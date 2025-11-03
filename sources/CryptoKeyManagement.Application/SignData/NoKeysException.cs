namespace DustInTheWind.CryptoKeyManagement.Application.SignData;

public class NoKeysException : Exception
{
    public NoKeysException()
        : base("No keys available. Please create a key first.")
    {
    }
}
