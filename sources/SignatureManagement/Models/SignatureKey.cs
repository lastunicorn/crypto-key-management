using System;

namespace DustInTheWind.SignatureManagement.Models
{
    internal class SignatureKey
    {
        public Guid Id { get; set; }

        public string PrivateKey { get; set; }

        public string PublicKey { get; set; }
    }
}
