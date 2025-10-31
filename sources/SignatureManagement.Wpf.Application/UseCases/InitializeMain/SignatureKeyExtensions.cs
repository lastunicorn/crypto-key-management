using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;

internal static class SignatureKeyExtensions
{
    public static SignatureKeyDto ToDto(this SignatureKey key)
    {
        if (key == null)
            return null;

        return new SignatureKeyDto
        {
            Id = key.Id,
            CreatedDate = key.CreatedDate,
            PrivateKey = key.PrivateKey,
            PublicKey = key.PublicKey
        };
    }
}