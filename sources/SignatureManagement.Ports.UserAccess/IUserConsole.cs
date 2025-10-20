namespace DustInTheWind.SignatureManagement.Ports.UserAccess;

public interface IUserConsole
{
    void DisplaySignatures(IEnumerable<SignatureSummary> signatures);
    
    Guid? AskSignatureId();
}