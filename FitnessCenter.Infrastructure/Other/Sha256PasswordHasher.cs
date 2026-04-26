using FitnessCenter.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCenter.Infrastructure.Other
{
    public class Sha256PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 10000, HashAlgorithmName.SHA256, 32);
            byte[] hashBytes = new byte[SaltSize + 32];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, 32);
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hash)
        {
            byte[] hashBytes = Convert.FromBase64String(hash);
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);
            byte[] storedHash = new byte[32];
            Buffer.BlockCopy(hashBytes, SaltSize, storedHash, 0, 32);
            byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 10000, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }
    }
}
