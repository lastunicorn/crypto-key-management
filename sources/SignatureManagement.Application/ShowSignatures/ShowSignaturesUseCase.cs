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

        List<SignatureKeyInfo> signatures = signatureRepository.GetAvailableSignatures().ToList();

        if (!signatures.Any())
        {
            Console.WriteLine("No signatures found.\n");
            return Task.FromResult((object)null);
        }

        foreach (SignatureKeyInfo signature in signatures)
        {
            Console.WriteLine($"ID: {signature.Id}");

            Console.WriteLine($"  Private Key Path: {signature.PrivateKeyPath}");
            Console.WriteLine($"  Private Key Value: {Convert.ToBase64String(signature.PrivateKey)}");

            Console.WriteLine($"  Public Key Path: {signature.PublicKeyPath}");
            Console.WriteLine($"  Public Key Value: {Convert.ToBase64String(signature.PublicKey)}");

            Console.WriteLine($"  Created: {File.GetCreationTime(signature.PrivateKeyPath):yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine();
        }

        return Task.FromResult((object)null);
    }
}