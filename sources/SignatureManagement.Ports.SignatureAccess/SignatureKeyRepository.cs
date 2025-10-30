using DustInTheWind.SignatureManagement.Domain;

namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public class SignatureKeyRepository : ISignatureKeyRepository
{
    private const string SignaturesDirectory = "signature-keys";

    public IEnumerable<SignatureKey> GetAll()
    {
        EnsureSignaturesDirectoryExists();

        List<SignatureKey> signatures = [];

        if (!Directory.Exists(SignaturesDirectory))
            return signatures;

        string[] privateKeyPaths = Directory.GetFiles(SignaturesDirectory, "*_private.key");

        foreach (string privateKeyPath in privateKeyPaths)
        {
            string fileName = Path.GetFileNameWithoutExtension(privateKeyPath);
            string guidPart = fileName.Replace("_private", "");

            if (Guid.TryParse(guidPart, out Guid id))
            {
                string publicKeyPath = Path.Combine(SignaturesDirectory, $"{id}_public.key");

                if (File.Exists(publicKeyPath))
                {
                    signatures.Add(new SignatureKey
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
        }

        return signatures;
    }

    public SignatureKey GetById(Guid id)
    {
        EnsureSignaturesDirectoryExists();

        string privateKeyPath = Path.Combine(SignaturesDirectory, $"{id}_private.key");
        string publicKeyPath = Path.Combine(SignaturesDirectory, $"{id}_public.key");

        if (!File.Exists(privateKeyPath) || !File.Exists(publicKeyPath))
            return null;

        return new SignatureKey
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

        string privateKeyPath = Path.Combine(SignaturesDirectory, $"{signatureId}_private.key");
        File.WriteAllText(privateKeyPath, Convert.ToBase64String(privateKey));

        string publicKeyPath = Path.Combine(SignaturesDirectory, $"{signatureId}_public.key");
        File.WriteAllText(publicKeyPath, Convert.ToBase64String(publicKey));

        return signatureId;
    }

    private static void EnsureSignaturesDirectoryExists()
    {
        if (!Directory.Exists(SignaturesDirectory))
            Directory.CreateDirectory(SignaturesDirectory);
    }
}
