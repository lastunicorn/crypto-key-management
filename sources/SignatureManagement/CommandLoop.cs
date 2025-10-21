using AsyncMediator;
using DustInTheWind.SignatureManagement.Application.CreateSignature;
using DustInTheWind.SignatureManagement.Application.ShowSignatures;
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

    private void WriteLineColor(ConsoleColor foregroundColor, string text)
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
        Console.WriteLine("1. Create Signature");
        Console.WriteLine("2. Show Signatures");
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
            case "create signature":
                {
                    CreateSignatureCommand command = new();
                    ICommandWorkflowResult result = await mediator.Send(command)
                        .ConfigureAwait(false);

                    CreateSignatureResponse response = result.Result<CreateSignatureResponse>();

                    WriteLineColor(ConsoleColor.Green, "✓ Signature created successfully!");
                    Console.WriteLine($"  Signature ID: {response.KeyId}");
                    Console.WriteLine($"  Private Key saved to: {response.PrivateKeyPath}");
                    Console.WriteLine($"  Public Key saved to: {response.PublicKeyPath}");
                    Console.WriteLine($"  Private Key Length: {response.PrivateKey.Length} bytes");
                    Console.WriteLine($"  Public Key Length: {response.PublicKey.Length} bytes\n");

                    break;
                }

            case "2":
            case "show":
            case "show signatures":
                {
                    ShowSignaturesCriteria criteria = new();
                    ShowSignaturesResponse response = await mediator.Query<ShowSignaturesCriteria, ShowSignaturesResponse>(criteria)
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

    private static void DisplaySignatures(ShowSignaturesResponse response)
    {
        Console.WriteLine("Signatures:");

        if (!response.Signatures.Any())
        {
            Console.WriteLine("No signatures found.\n");
            return;
        }

        foreach (SignatureDetails signature in response.Signatures)
        {
            Console.WriteLine($"ID: {signature.Id}");

            Console.WriteLine($"  Private Key Path: {signature.PrivateKeyPath}");
            Console.WriteLine($"  Private Key Value: {signature.PrivateKeyValue}");

            Console.WriteLine($"  Public Key Path: {signature.PublicKeyPath}");
            Console.WriteLine($"  Public Key Value: {signature.PublicKeyValue}");

            Console.WriteLine($"  Created: {signature.Created:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
        }
    }

    protected virtual void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}
