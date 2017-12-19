using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibleAppCore.Utilities.ExtensionMethods
{
    public static partial class ExtensionsClass
    {
        public static string HashString(this string inputString, string saltString)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            //TODO ograniczyc do 128 / 8 bytow
            salt = Encoding.ASCII.GetBytes(saltString);


            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: inputString,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 512 / 8));

            return hashed;
        }
    }
}
