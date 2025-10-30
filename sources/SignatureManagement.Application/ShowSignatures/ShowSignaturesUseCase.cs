using AsyncMediator;
using DustInTheWind.SignatureManagement.Domain;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace DustInTheWind.SignatureManagement.Application.ShowSignatures;

internal class ShowSignaturesUseCase : IQuery<ShowSignaturesCriteria, ShowSignaturesResponse>
{
    private readonly ISignatureKeyRepository signatureRepository;

    public ShowSignaturesUseCase(ISignatureKeyRepository signatureRepository)
    {
        this.signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
    }

    public Task<ShowSignaturesResponse> Query(ShowSignaturesCriteria criteria)
    {
        List<SignatureKey> signatures = signatureRepository.GetAll()
            .ToList();

        IEnumerable<SignatureDetails> signatureDetails = signatures
            .Select(x => ToSignatureDetails(x));

        var response = new ShowSignaturesResponse
        {
            Signatures = signatureDetails
        };

        return Task.FromResult(response);
    }

    private static SignatureDetails ToSignatureDetails(SignatureKey x)
    {
        return new SignatureDetails
        {
            Id = x.Id,
            PrivateKeyValue = Convert.ToBase64String(x.PrivateKey),
            PublicKeyValue = Convert.ToBase64String(x.PublicKey),
            Created = x.CreatedDate
        };
    }
}