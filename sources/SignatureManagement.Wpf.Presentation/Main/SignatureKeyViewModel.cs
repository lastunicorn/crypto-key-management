using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

public class SignatureKeyViewModel
{
    public Guid Id { get; set; }
    
    public string CreatedDateText { get; set; }

    public static SignatureKeyViewModel FromSignatureKey(SignatureKey signatureKey)
    {
        return new SignatureKeyViewModel
        {
            Id = signatureKey.Id,
            CreatedDateText = $"Created: {signatureKey.CreatedDate:yyyy-MM-dd HH:mm:ss}",
        };
    }
}