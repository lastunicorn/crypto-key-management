using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;

internal static class KeyPairDtoExtensions
{
    public static IEnumerable<KeyPairViewModel> ToViewModels(this IEnumerable<KeyPairDto> signatureKeyDtos)
    {
        foreach (KeyPairDto keyPairDto in signatureKeyDtos)
            yield return new KeyPairViewModel
            {
                Id = keyPairDto.Id,
                CreatedDateText = $"Created: {keyPairDto.CreatedDate:yyyy-MM-dd HH:mm:ss}",
                PrivateKeyBase64 = Convert.ToBase64String(keyPairDto.PrivateKey),
                PublicKeyBase64 = Convert.ToBase64String(keyPairDto.PublicKey)
            };
    }
}
