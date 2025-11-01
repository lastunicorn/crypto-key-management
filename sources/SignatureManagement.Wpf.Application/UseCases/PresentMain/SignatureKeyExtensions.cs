using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Wpf.Application.UseCases.PresentMain;

internal static class SignatureKeyExtensions
{
    public static KeyPairDto ToDto(this KeyPair key)
    {
        if (key == null)
            return null;

        return new KeyPairDto
        {
            Id = key.Id,
            CreatedDate = key.CreatedDate,
            PrivateKey = key.PrivateKey,
            PublicKey = key.PublicKey
        };
    }
}