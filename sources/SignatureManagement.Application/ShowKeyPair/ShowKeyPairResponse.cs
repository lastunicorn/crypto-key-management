namespace DustInTheWind.SignatureManagement.Application.ShowKeyPair;

public class ShowKeyPairResponse
{
    public IEnumerable<KeyPairDetails> Signatures { get; set; }
}
