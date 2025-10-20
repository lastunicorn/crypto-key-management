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

        foreach (var privateKeyFile in privateKeyFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(privateKeyFile);
            string guidPart = fileName.Replace("_private", "");

            if (Guid.TryParse(guidPart, out Guid id))
            {
                string publicKeyFile = Path.Combine(SignaturesDirectory, $"{id}_public.key");

                if (File.Exists(publicKeyFile))
                    signatures.Add(new SignatureKeyInfo
                    {
                        Id = id,
                        PrivateKeyPath = privateKeyFile,
                        PublicKeyPath = publicKeyFile
                    });
            }
        }

        return signatures
            .OrderBy(x => x.Id)
            .ToList();
    }

    public void SaveSignatureKey(Guid signatureId, Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey, out string privateKeyPath, out string publicKeyPath)
    {
        EnsureSignaturesDirectoryExists();

        // Save keys to files
        privateKeyPath = Path.Combine(SignaturesDirectory, $"{signatureId}_private.key");
        publicKeyPath = Path.Combine(SignaturesDirectory, $"{signatureId}_public.key");
        File.WriteAllText(privateKeyPath, Convert.ToBase64String(privateKey.GetEncoded()));
        File.WriteAllText(publicKeyPath, Convert.ToBase64String(publicKey.GetEncoded()));
    }


    private static void EnsureSignaturesDirectoryExists()
    {
        if (!Directory.Exists(SignaturesDirectory))
            Directory.CreateDirectory(SignaturesDirectory);
    }
}
