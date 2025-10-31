using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

internal static class KeyPairDtoExtensions
{
    public static IEnumerable<KeyPairViewModel> ToViewModels(this IEnumerable<KeyPairDto> signatureKeyDtos)
    {
        foreach (var dto in signatureKeyDtos)
        {
            yield return new KeyPairViewModel
            {
                Id = dto.Id,
                CreatedDateText = $"Created: {dto.CreatedDate:yyyy-MM-dd HH:mm:ss}",
                PrivateKeyBase64 = Convert.ToBase64String(dto.PrivateKey),
                PublicKeyBase64 = Convert.ToBase64String(dto.PublicKey)
            };
        }
    }
}
