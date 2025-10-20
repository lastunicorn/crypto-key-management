using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;

namespace BouncyCastleDemo
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Ed25519 Signature Generation and Verification Test");
            Console.WriteLine("==================================================\n");

            Console.WriteLine("1. Generating Ed25519 key pair...");
            Ed25519KeyPairGenerator keyPairGenerator = new Ed25519KeyPairGenerator();
            keyPairGenerator.Init(new Ed25519KeyGenerationParameters(new SecureRandom()));
            AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();

            Ed25519PrivateKeyParameters privateKey = (Ed25519PrivateKeyParameters)keyPair.Private;
            Ed25519PublicKeyParameters publicKey = (Ed25519PublicKeyParameters)keyPair.Public;

            Console.WriteLine($"   Private Key Length: {privateKey.GetEncoded().Length} bytes");
            Console.WriteLine($"   Private Key (Base64): {Convert.ToBase64String(privateKey.GetEncoded())}\n");

            Console.WriteLine($"   Public Key Length: {publicKey.GetEncoded().Length} bytes");
            Console.WriteLine($"   Public Key (Base64): {Convert.ToBase64String(publicKey.GetEncoded())}\n");

            Console.WriteLine("2. Signing test message...");
            string testMessage = "This is a test message for Ed25519 signature verification.";
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(testMessage);

            Ed25519Signer signer = new Ed25519Signer();
            signer.Init(true, privateKey);
            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
            byte[] signature = signer.GenerateSignature();

            Console.WriteLine($"   Message: {testMessage}");
            Console.WriteLine($"   Signature Length: {signature.Length} bytes");
            Console.WriteLine($"   Signature (Base64): {Convert.ToBase64String(signature)}\n");

            Console.WriteLine("3. Verifying signature with original message...");
            Ed25519Signer verifier = new Ed25519Signer();
            verifier.Init(false, publicKey);
            verifier.BlockUpdate(messageBytes, 0, messageBytes.Length);
            bool isValid = verifier.VerifySignature(signature);

            Console.WriteLine($"   Signature Valid: {isValid}\n");

            Console.WriteLine("4. Testing with modified message (should fail)...");
            string modifiedMessage = "This is a MODIFIED test message for Ed25519 signature verification.";
            byte[] modifiedMessageBytes = System.Text.Encoding.UTF8.GetBytes(modifiedMessage);

            Ed25519Signer verifier2 = new Ed25519Signer();
            verifier2.Init(false, publicKey);
            verifier2.BlockUpdate(modifiedMessageBytes, 0, modifiedMessageBytes.Length);
            bool isValidModified = verifier2.VerifySignature(signature);

            Console.WriteLine($"   Modified Message: {modifiedMessage}");
            Console.WriteLine($"   Signature Valid: {isValidModified}\n");

            Console.WriteLine("==================================================");
            Console.WriteLine("Test Results:");
            Console.WriteLine($"  ✓ Key pair generation: Success");
            Console.WriteLine($"  ✓ Message signing: Success");
            Console.WriteLine($"  ✓ Signature verification: {(isValid ? "PASS" : "FAIL")}");
            Console.WriteLine($"  ✓ Modified message rejection: {(!isValidModified ? "PASS" : "FAIL")}");
            Console.WriteLine($"\nAll tests: {(isValid && !isValidModified ? "PASSED ✓" : "FAILED ✗")}");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
