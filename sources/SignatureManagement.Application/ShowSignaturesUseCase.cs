using System;
using System.IO;
using System.Linq;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace DustInTheWind.SignatureManagement.Application
{
    public class ShowSignaturesUseCase
    {
        private readonly ISignatureRepository signatureRepository;

        public ShowSignaturesUseCase(ISignatureRepository signatureRepository)
        {
            this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
        }

        public void Execute()
        {
            Console.WriteLine("Existing Signatures:");
            Console.WriteLine("===================");

            var signatures = signatureRepository.GetAvailableSignatures();

            if (!signatures.Any())
            {
                Console.WriteLine("No signatures found.\n");
                return;
            }

            foreach (var signature in signatures)
            {
                Console.WriteLine($"ID: {signature.Id}");
                Console.WriteLine($"  Private Key: {signature.PrivateKeyPath}");
                Console.WriteLine($"  Public Key: {signature.PublicKeyPath}");
                Console.WriteLine($"  Created: {File.GetCreationTime(signature.PrivateKeyPath):yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine();
            }
        }
    }
}