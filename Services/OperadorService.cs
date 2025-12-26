using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebApp.Services
{
    public interface IOperadorService
    {
        string HashPin(string pin);
        bool VerifyPin(string pin, string hashedPin);
    }

    public class OperadorService : IOperadorService
    {
        public string HashPin(string pin)
        {
            // Generar un salt aleatorio de 128 bits
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash el PIN usando PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pin,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Combinar salt y hash
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        public bool VerifyPin(string pin, string hashedPin)
        {
            try
            {
                var parts = hashedPin.Split('.');
                if (parts.Length != 2)
                    return false;

                var salt = Convert.FromBase64String(parts[0]);
                var hash = parts[1];

                // Hash el PIN ingresado con el mismo salt
                string hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: pin,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                return hash == hashedInput;
            }
            catch
            {
                return false;
            }
        }
    }
}