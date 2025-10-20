namespace DustInTheWind.SignatureManagement.Ports.UserAccess;

public interface IUserConsole
{
    void DisplaySignatures(IEnumerable<SignatureInfo> signatures);
}