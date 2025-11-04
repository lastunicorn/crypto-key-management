using DustInTheWind.CryptoKeyManagement.Domain;

namespace DustInTheWind.CryptoKeyManagement.Ports.CryptoKeyAccess;

public class CryptoKeyRepository : ICryptoKeyRepository
{
    private readonly string directoryPath;

    public CryptoKeyRepository()
    {
        string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appDataFolder, "Crypto Key Management");
        directoryPath = Path.Combine(appFolder, "Crypto Keys");
    }

    public IEnumerable<KeyPair> GetAll()
    {
        EnsureSignaturesDirectoryExists();

        List<KeyPair> signatures = [];

        if (!Directory.Exists(directoryPath))
            return signatures;

        string[] privateKeyPaths = Directory.GetFiles(directoryPath, "*_private.key");

        foreach (string privateKeyPath in privateKeyPaths)
        {
            string fileName = Path.GetFileNameWithoutExtension(privateKeyPath);
            string guidPart = fileName.Replace("_private", "");

            if (Guid.TryParse(guidPart, out Guid id))
            {
                string publicKeyPath = Path.Combine(directoryPath, $"{id}_public.key");

                if (File.Exists(publicKeyPath))
                    signatures.Add(new KeyPair
                    {
                        Id = id,
                        PrivateKeyPath = privateKeyPath,
                        PrivateKey = Convert.FromBase64String(File.ReadAllText(privateKeyPath)),
                        PublicKeyPath = publicKeyPath,
                        PublicKey = Convert.FromBase64String(File.ReadAllText(publicKeyPath)),
                        CreatedDate = File.GetCreationTime(privateKeyPath)
                    });
            }
        }

        return signatures;
    }

    public KeyPair GetById(Guid id)
    {
        EnsureSignaturesDirectoryExists();

        string privateKeyPath = Path.Combine(directoryPath, $"{id}_private.key");
        string publicKeyPath = Path.Combine(directoryPath, $"{id}_public.key");

        if (!File.Exists(privateKeyPath) || !File.Exists(publicKeyPath))
            return null;

        return new KeyPair
        {
            Id = id,
            PrivateKeyPath = privateKeyPath,
            PrivateKey = Convert.FromBase64String(File.ReadAllText(privateKeyPath)),
            PublicKeyPath = publicKeyPath,
            PublicKey = Convert.FromBase64String(File.ReadAllText(publicKeyPath)),
            CreatedDate = File.GetCreationTime(privateKeyPath)
        };
    }

    public Guid Add(byte[] privateKey, byte[] publicKey)
    {
        EnsureSignaturesDirectoryExists();

        Guid signatureId = Guid.NewGuid();

        string privateKeyPath = Path.Combine(directoryPath, $"{signatureId}_private.key");
        File.WriteAllText(privateKeyPath, Convert.ToBase64String(privateKey));

        string publicKeyPath = Path.Combine(directoryPath, $"{signatureId}_public.key");
        File.WriteAllText(publicKeyPath, Convert.ToBase64String(publicKey));

        return signatureId;
    }

    public void Delete(Guid id)
    {
        string privateKeyPath = Path.Combine(directoryPath, $"{id}_private.key");
        string publicKeyPath = Path.Combine(directoryPath, $"{id}_public.key");

        if (File.Exists(privateKeyPath))
            File.Delete(privateKeyPath);

        if (File.Exists(publicKeyPath))
            File.Delete(publicKeyPath);
    }

    private void EnsureSignaturesDirectoryExists()
    {
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
    }
}
