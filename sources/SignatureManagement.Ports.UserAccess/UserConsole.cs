namespace DustInTheWind.SignatureManagement.Ports.UserAccess;

public class UserConsole : IUserConsole
{
    public void DisplaySignatures(IEnumerable<SignatureSummary> signatures)
    {
        Console.WriteLine("Existing Signatures:");
        Console.WriteLine("===================");

        if (!signatures.Any())
        {
            Console.WriteLine("No signatures found.\n");
            return;
        }

        foreach (SignatureSummary signature in signatures)
        {
            Console.WriteLine($"ID: {signature.Id}");
            Console.WriteLine($"  Created: {signature.CreatedDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
        }
    }
}
