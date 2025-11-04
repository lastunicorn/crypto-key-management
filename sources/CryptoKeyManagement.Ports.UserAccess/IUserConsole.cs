namespace DustInTheWind.CryptoKeyManagement.Ports.UserAccess;

public interface IUserConsole
{
    void DisplayKeyPairs(IEnumerable<KeyPairSummary> keyPairSummaries);
    
    Guid? AskKeyPairId();
    
    string AskForDataToSign();
}