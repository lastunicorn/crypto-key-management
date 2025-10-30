using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;

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
    public static IEnumerable<SignatureKeyViewModel> ToViewModels(this IEnumerable<SignatureKeyDto> signatureKeyDtos)
    {
        foreach (var dto in signatureKeyDtos)
        {
            yield return new SignatureKeyViewModel
            {
                Id = dto.Id,
                CreatedDateText = $"Created: {dto.CreatedDate:yyyy-MM-dd HH:mm:ss}",
                PrivateKeyBase64 = Convert.ToBase64String(dto.PrivateKey),
                PublicKeyBase64 = Convert.ToBase64String(dto.PublicKey)
            };
        }
    }
}