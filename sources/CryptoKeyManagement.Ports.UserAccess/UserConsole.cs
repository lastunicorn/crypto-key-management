using System.Collections.Generic;

namespace DustInTheWind.CryptoKeyManagement.Ports.UserAccess;

public class UserConsole : IUserConsole
{
    public void DisplaySignatures(IEnumerable<SignatureSummary> signatures)
    {
        Console.WriteLine("Keys:");

        if (!signatures.Any())
        {
            WriteLineColor(ConsoleColor.DarkYellow, "No keys found.\n");
            return;
        }

        foreach (SignatureSummary signature in signatures)
        {
            Console.WriteLine($"ID: {signature.Id}");
            Console.WriteLine($"  Private Key: {Convert.ToBase64String(signature.PrivateKey)}");
            Console.WriteLine($"  Public Key: {Convert.ToBase64String(signature.PublicKey)}");

            Console.WriteLine($"  Created: {signature.CreatedDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
        }
    }

    public Guid? AskSignatureId()
    {
        WriteColor(ConsoleColor.White, "\nEnter key ID to use (GUID): ");
        string rawValue = Console.ReadLine()?.Trim();

        if (Guid.TryParse(rawValue, out Guid signatureId))
            return signatureId;

        return null;
    }

    public string AskForDataToSign()
    {
        Console.WriteLine();
        WriteLineColor(ConsoleColor.White, "Enter data to sign (Ctrl+Z on new line to finish): ");
        
        List<string> lines = [];
        string line;
        
        while ((line = Console.ReadLine()) != null)
            lines.Add(line);
        
        return string.Join(Environment.NewLine, lines);
    }

    private static void WriteLineColor(ConsoleColor foregroundColor, string text)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(text);
        Console.ForegroundColor = oldColor;
    }

    private static void WriteColor(ConsoleColor foregroundColor, string text)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.Write(text);
        Console.ForegroundColor = oldColor;
    }
}
