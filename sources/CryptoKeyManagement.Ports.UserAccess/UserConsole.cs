using System.Collections.Generic;

namespace DustInTheWind.CryptoKeyManagement.Ports.UserAccess;

public class UserConsole : IUserConsole
{
    public void DisplayKeyPairs(IEnumerable<KeyPairSummary> keyPairSummaries)
    {
        Console.WriteLine("Keys:");

        if (!keyPairSummaries.Any())
        {
            WriteLineColor(ConsoleColor.DarkYellow, "No keys found.\n");
            return;
        }

        foreach (KeyPairSummary keyPairSummary in keyPairSummaries)
        {
            Console.WriteLine($"ID: {keyPairSummary.Id}");
            Console.WriteLine($"  Private Key: {Convert.ToBase64String(keyPairSummary.PrivateKey)}");
            Console.WriteLine($"  Public Key: {Convert.ToBase64String(keyPairSummary.PublicKey)}");

            Console.WriteLine($"  Created: {keyPairSummary.CreatedDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
        }
    }

    public Guid? AskKeyPairId()
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
