namespace DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.PresentSigningPage;

public class PresentSigningPageResponse
{
    public List<KeyPairDto> KeyPairs { get; set; }

    public Guid? SelectedKeyPairId { get; set; }
}
