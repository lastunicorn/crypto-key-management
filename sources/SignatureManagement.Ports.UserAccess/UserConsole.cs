namespace DustInTheWind.SignatureManagement.Ports.UserAccess;

public class UserConsole : IUserConsole
{
    public void DisplaySignatures(IEnumerable<SignatureSummary> signatures)
    {
        Console.WriteLine("Keys:");

        if (!signatures.Any())
        {
            Console.WriteLine("No keys found.\n");
            return;
        }

        foreach (SignatureSummary signature in signatures)
            Console.WriteLine($"- [{signature.CreatedDate:yyyy-MM-dd HH:mm:ss}] {signature.Id}");
    }

    public Guid? AskSignatureId()
    {
        Console.Write("\nEnter key ID (GUID): ");
        string rawValue = Console.ReadLine()?.Trim();

        if (Guid.TryParse(rawValue, out Guid signatureId))
            return signatureId;

        return null;
    }

    public string AskForDataToSign()
    {
        Console.Write("Enter data to sign: ");
        return Console.ReadLine();
    }
}
