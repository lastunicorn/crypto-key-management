namespace DustInTheWind.CryptoKeyManagement.Ports.UserAccess;

public interface IUserConsole
{
    void DisplaySignatures(IEnumerable<SignatureSummary> signatures);
    
    Guid? AskSignatureId();
    
    string AskForDataToSign();
}