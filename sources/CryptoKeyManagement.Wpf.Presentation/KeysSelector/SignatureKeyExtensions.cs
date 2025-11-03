using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentMain;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.KeysSelector;

/// <summary>
/// Extension methods for converting signature key DTOs to view models.
/// </summary>
internal static class SignatureKeyExtensions
{
    /// <summary>
    /// Converts a collection of signature key DTOs to view models.
    /// </summary>
    /// <param name="signatureKeyDtos">The collection of signature key DTOs to convert.</param>
    /// <returns>A collection of signature key view models.</returns>
    public static IEnumerable<SignatureKeyViewModel> ToViewModels(this IEnumerable<KeyPairDto> signatureKeyDtos)
    {
        foreach (KeyPairDto keyPairDto in signatureKeyDtos)
            yield return new SignatureKeyViewModel
            {
                Id = keyPairDto.Id,
                CreatedDateText = $"Created: {keyPairDto.CreatedDate:yyyy-MM-dd HH:mm:ss}",
                PrivateKeyBase64 = Convert.ToBase64String(keyPairDto.PrivateKey),
                PublicKeyBase64 = Convert.ToBase64String(keyPairDto.PublicKey)
            };
    }
}