using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace DustInTheWind.SignatureManagement.Wpf.Application.InitializeMain;

internal static class SignatureKeyExtensions
{
    public static SignatureKeyDto ToDto(this SignatureKey key)
    {
        return new SignatureKeyDto
        {
            Id = key.Id,
            CreatedDate = key.CreatedDate,
            PrivateKey = key.PrivateKey,
            PublicKey = key.PublicKey
        };
    }
}