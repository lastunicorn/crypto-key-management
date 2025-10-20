using System;
using System.Threading.Tasks;
using DustInTheWind.SignatureManagement.Application;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace DustInTheWind.SignatureManagement
{
    internal class CommandLoop
    {
        public event EventHandler Closed;

        public Task RunAsync()
        {
            return Task.Run(() => Run());
        }

        public void Run()
        {
            while (true)
            {
                ShowMenu();
                string command = Console.ReadLine()?.Trim().ToLowerInvariant();
                Console.WriteLine();

                bool flowControl = ProcessCommand(command);
                if (!flowControl)
                    return;
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

        private bool ProcessCommand(string command)
        {
            switch (command)
            {
                case "1":
                case "create":
                case "create signature":
                    {
                        ISignatureRepository signatureRepository = new SignatureRepository();
                        CreateSignatureUseCase createSignatureUseCase = new CreateSignatureUseCase(signatureRepository);
                        createSignatureUseCase.Execute();
                        break;
                    }

                case "2":
                case "show":
                case "show signatures":
                    {
                        SignatureRepository signatureRepository = new SignatureRepository();
                        ShowSignaturesUseCase showSignaturesUseCase = new ShowSignaturesUseCase(signatureRepository);
                        showSignaturesUseCase.Execute();
                        break;
                    }

                case "3":
                case "sign":
                    {
                        SignatureRepository signatureRepository = new SignatureRepository();
                        SignDataUseCase signDataUseCase = new SignDataUseCase(signatureRepository);
                        signDataUseCase.Execute();
                        break;
                    }

                case "0":
                case "exit":
                case "quit":
                    Console.WriteLine("Goodbye!");
                    OnClosed();
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
}
