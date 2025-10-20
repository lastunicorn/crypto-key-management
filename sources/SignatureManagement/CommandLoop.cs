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
                    CreateSignatureCommand command = new CreateSignatureCommand();
                    await mediator.Send(command).ConfigureAwait(false);
                    break;
                }

            case "2":
            case "show":
            case "show signatures":
                {
                    ShowSignaturesCriteria criteria = new ShowSignaturesCriteria();
                    await mediator.Query<ShowSignaturesCriteria, object>(criteria).ConfigureAwait(false);
                    break;
                }

            case "3":
            case "sign":
                {
                    SignDataCriteria criteria = new SignDataCriteria();
                    await mediator.Query<SignDataCriteria, SignDataResult>(criteria).ConfigureAwait(false);
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
