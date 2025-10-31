using AsyncMediator;
using DustInTheWind.SignatureManagement.Application.CreateKeyPair;
using DustInTheWind.SignatureManagement.Application.ShowKeyPair;
using DustInTheWind.SignatureManagement.Application.SignData;

namespace DustInTheWind.SignatureManagement;

internal class CommandLoop
{
    private readonly IMediator mediator;

    public event EventHandler Closed;

    public CommandLoop(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ShowMenu();

            string command = Console.ReadLine()?.Trim().ToLowerInvariant();
            Console.WriteLine();

            try
            {
                bool flowControl = await ProcessCommand(command);
                if (!flowControl)
                {
                    OnClosed();
                    return;
                }
            }
            catch (Exception ex)
            {
                WriteLineColor(ConsoleColor.Red, $"Error: {ex.Message}\n");
            }
        }
    }

    private static void WriteLineColor(ConsoleColor foregroundColor, string text)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(text);
        Console.ForegroundColor = oldColor;
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Available Commands");
        Console.WriteLine("------------------");
        Console.WriteLine("1. Create Keys");
        Console.WriteLine("2. Show Keys");
        Console.WriteLine("3. Sign Data");
        Console.WriteLine("0. Exit");
        Console.Write("\nEnter command (number or name): ");
    }

    private async Task<bool> ProcessCommand(string cliCommand)
    {
        switch (cliCommand)
        {
            case "1":
            case "create":
            case "create keys":
                {
                    CreateKeyPairRequest command = new();
                    ICommandWorkflowResult result = await mediator.Send(command)
                        .ConfigureAwait(false);

                    CreateKeyPairResponse response = result.Result<CreateKeyPairResponse>();

                    WriteLineColor(ConsoleColor.Green, "✓ Keys created successfully!");
                    Console.WriteLine($"  Key ID: {response.KeyId}");
                    Console.WriteLine($"  Private Key saved to: {response.PrivateKeyPath}");
                    Console.WriteLine($"  Public Key saved to: {response.PublicKeyPath}");
                    Console.WriteLine($"  Private Key Length: {response.PrivateKey.Length} bytes");
                    Console.WriteLine($"  Public Key Length: {response.PublicKey.Length} bytes\n");

                    break;
                }

            case "2":
            case "show":
            case "show keys":
                {
                    ShowKeyPairRequest criteria = new();
                    ShowKeyPairResponse response = await mediator.Query<ShowKeyPairRequest, ShowKeyPairResponse>(criteria)
                        .ConfigureAwait(false);
                    
                    DisplaySignatures(response);
                    break;
                }

            case "3":
            case "sign":
                {
                    SignDataCriteria criteria = new();
                    SignDataResponse response = await mediator.Query<SignDataCriteria, SignDataResponse>(criteria)
                        .ConfigureAwait(false);

                    WriteLineColor(ConsoleColor.Green, "\n✓ Data signed successfully!");
                    Console.WriteLine($"Signature (Base64): {Convert.ToBase64String(response.Signature)}\n");

                    break;
                }

            case "0":
            case "exit":
            case "quit":
                Console.WriteLine("Goodbye!");
                return false;

            default:
                Console.WriteLine("Invalid command. Please try again.\n");
                break;
        }

        return true;
    }

    private static void DisplaySignatures(ShowKeyPairResponse response)
    {
        Console.WriteLine("Keys:");

        if (!response.Signatures.Any())
        {
            WriteLineColor(ConsoleColor.DarkYellow, "No keys found.\n");
            return;
        }

        foreach (KeyPairDetails signature in response.Signatures)
        {
            Console.WriteLine($"ID: {signature.Id}");
            Console.WriteLine($"  Private Key: {signature.PrivateKeyValue}");
            Console.WriteLine($"  Public Key: {signature.PublicKeyValue}");

            Console.WriteLine($"  Created: {signature.Created:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
        }
    }

    protected virtual void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}
