using Org.BouncyCastle.Crypto.Parameters;

namespace DustInTheWind.SignatureManagement.Ports.SignatureAccess;

public class SignatureRepository : ISignatureRepository
{
    private const string SignaturesDirectory = "signatures";

    public List<SignatureKeyInfo> GetAvailableSignatures()
    {
        EnsureSignaturesDirectoryExists();

        var signatures = new List<SignatureKeyInfo>();

        if (!Directory.Exists(SignaturesDirectory))
            return signatures;

        var privateKeyFiles = Directory.GetFiles(SignaturesDirectory, "*_private.key");

        foreach (var privateKeyFilePath in privateKeyFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(privateKeyFilePath);
            string guidPart = fileName.Replace("_private", "");

            if (Guid.TryParse(guidPart, out Guid id))
            {
                string publicKeyFilePath = Path.Combine(SignaturesDirectory, $"{id}_public.key");

                if (File.Exists(publicKeyFilePath))
                {
                    signatures.Add(new SignatureKeyInfo
                    {
                        Id = id,
                        PrivateKeyPath = privateKeyFilePath,
                        PrivateKey = Convert.FromBase64String(File.ReadAllText(privateKeyFilePath)),
                        PublicKeyPath = publicKeyFilePath,
                        PublicKey = Convert.FromBase64String(File.ReadAllText(publicKeyFilePath))
                    });
                }
            }
        }

        return signatures
            .OrderBy(x => x.Id)
            .ToList();
    }

    public SignatureKeyInfo GetSignatureById(Guid id)
    {
        EnsureSignaturesDirectoryExists();

        string privateKeyPath = Path.Combine(SignaturesDirectory, $"{id}_private.key");
        string publicKeyPath = Path.Combine(SignaturesDirectory, $"{id}_public.key");

        if (!File.Exists(privateKeyPath) || !File.Exists(publicKeyPath))
            return null;

        return new SignatureKeyInfo
        {
            Id = id,
            PrivateKeyPath = privateKeyPath,
            PrivateKey = Convert.FromBase64String(File.ReadAllText(privateKeyPath)),
            PublicKeyPath = publicKeyPath,
            PublicKey = Convert.FromBase64String(File.ReadAllText(publicKeyPath))
        };
    }

    public Guid SaveSignatureKey(Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey)
    {
        EnsureSignaturesDirectoryExists();

        // Generate GUID for this signature
        Guid signatureId = Guid.NewGuid();

        // Save keys to files
        string privateKeyPath = Path.Combine(SignaturesDirectory, $"{signatureId}_private.key");
        string publicKeyPath = Path.Combine(SignaturesDirectory, $"{signatureId}_public.key");
        File.WriteAllText(privateKeyPath, Convert.ToBase64String(privateKey.GetEncoded()));
        File.WriteAllText(publicKeyPath, Convert.ToBase64String(publicKey.GetEncoded()));

        return signatureId;
    }

    private static void EnsureSignaturesDirectoryExists()
    {
        if (!Directory.Exists(SignaturesDirectory))
            Directory.CreateDirectory(SignaturesDirectory);
    }
}
