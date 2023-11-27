using System.Security.Cryptography;

namespace AKDEMIC.CORE.Services
{
    public class RandomPasswordService : IRandomPasswordService
    {
        private readonly string PasswordAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz";
        private readonly int PasswordSize = 6;

        public string GeneratePassword()
        {
            using (var crypto = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[PasswordSize];

                // If chars.Length isn't a power of 2 then there is a bias if we simply use the modulus operator. The first characters of chars will be more probable than the last ones.
                // buffer used if we encounter an unusable random byte. We will regenerate it in this buffer
                byte[] buffer = null;

                // Maximum random number that can be used without introducing a bias
                int maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % PasswordAlphabet.Length);

                crypto.GetBytes(data);

                char[] result = new char[PasswordSize];

                for (int i = 0; i < PasswordSize; i++)
                {
                    byte value = data[i];

                    while (value > maxRandom)
                    {
                        if (buffer == null)
                        {
                            buffer = new byte[1];
                        }

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = PasswordAlphabet[value % PasswordAlphabet.Length];
                }

                return new string(result);
            }
        }
    }
}
