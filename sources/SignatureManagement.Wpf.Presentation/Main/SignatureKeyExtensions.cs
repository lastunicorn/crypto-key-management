using DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

internal static class SignatureKeyExtensions
{
    public static IEnumerable<SignatureKeyViewModel> ToViewModels(this IEnumerable<SignatureKeyDto> signatureKeyDtos)
    {
        foreach (var dto in signatureKeyDtos)
        {
            yield return new SignatureKeyViewModel
            {
                Id = dto.Id,
                CreatedDateText = $"Created: {dto.CreatedDate:yyyy-MM-dd HH:mm:ss}",
            };
        }
    }
}
