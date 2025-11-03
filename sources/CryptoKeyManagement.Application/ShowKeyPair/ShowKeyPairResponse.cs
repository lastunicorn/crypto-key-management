namespace DustInTheWind.CryptoKeyManagement.Application.ShowKeyPair;

public class ShowKeyPairResponse
{
    public IEnumerable<KeyPairDetails> Signatures { get; set; }
}
