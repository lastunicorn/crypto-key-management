using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace DustInTheWind.SignatureManagement.Application.ShowSignatures;

internal class ShowSignaturesUseCase : IQuery<ShowSignaturesCriteria, object>
{
    private readonly ISignatureRepository signatureRepository;

    public ShowSignaturesUseCase(ISignatureRepository signatureRepository)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
    }

    public Task<object> Query(ShowSignaturesCriteria criteria)
    {
        Console.WriteLine("Existing Signatures:");
        Console.WriteLine("===================");

        var signatures = signatureRepository.GetAvailableSignatures();

        if (!signatures.Any())
        {
            Console.WriteLine("No signatures found.\n");
            return Task.FromResult((object)null);
        }

        foreach (var signature in signatures)
        {
            Console.WriteLine($"ID: {signature.Id}");
            Console.WriteLine($"  Private Key: {signature.PrivateKeyPath}");
            Console.WriteLine($"  Public Key: {signature.PublicKeyPath}");
            Console.WriteLine($"  Created: {File.GetCreationTime(signature.PrivateKeyPath):yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
        }

        return Task.FromResult((object)null);
    }
}