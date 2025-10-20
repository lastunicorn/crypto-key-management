using AsyncMediator;
using DustInTheWind.SignatureManagement.Application.CreateSignature;
using DustInTheWind.SignatureManagement.Application.ShowSignatures;
using DustInTheWind.SignatureManagement.Application.SignData;
using Org.BouncyCastle.Asn1.Ocsp;

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
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}\n");
                Console.ForegroundColor = oldColor;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Available Commands:");
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

                    Console.WriteLine($"✓ Signature created successfully!");
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
                    await mediator.Query<ShowSignaturesCriteria, object>(criteria)
                        .ConfigureAwait(false);
                    break;
                }

            case "3":
            case "sign":
                {
                    SignDataCriteria criteria = new();
                    SignDataResponse response = await mediator.Query<SignDataCriteria, SignDataResponse>(criteria)
                        .ConfigureAwait(false);

                    Console.WriteLine("\n✓ Data signed successfully!");
                    Console.WriteLine($"Original Data: {response.OriginalData}");
                    Console.WriteLine($"Signature ID: {response.SignatureId}");
                    Console.WriteLine($"Signed Data (Base64): {Convert.ToBase64String(response.Signature)}\n");

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

    protected virtual void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}
