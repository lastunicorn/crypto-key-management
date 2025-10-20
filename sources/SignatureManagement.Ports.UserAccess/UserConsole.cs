namespace DustInTheWind.SignatureManagement.Ports.UserAccess;

public class UserConsole : IUserConsole
{
    public void DisplaySignatures(IEnumerable<SignatureSummary> signatures)
    {
        Console.WriteLine("Signatures:");

        if (!signatures.Any())
        {
            Console.WriteLine("No signatures found.\n");
            return;
        }

        foreach (SignatureSummary signature in signatures)
            Console.WriteLine($"- {signature.Id} ({signature.CreatedDate:yyyy-MM-dd HH:mm:ss})");
    }

    public Guid? AskSignatureId()
    {
        Console.Write("\nEnter Signature ID (GUID): ");
        string rawValue = Console.ReadLine()?.Trim();

        if (Guid.TryParse(rawValue, out Guid signatureId))
            return signatureId;

        return null;
    }
}
