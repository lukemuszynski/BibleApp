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

        public static string FindInternalOf(this string data, string start, string end)
        {
            int lengthToMove = start.Length;
            int indexOfBookShortName =
                data.IndexOf(start);
            if (indexOfBookShortName < 0)
                return "";
            indexOfBookShortName += lengthToMove;
            int indexOfEndTitle = data.IndexOf(end, indexOfBookShortName);
            if (indexOfEndTitle == -1)
            {
                end = "</div><div class=\"bottom-navi\"";
                indexOfEndTitle = data.IndexOf(end, indexOfBookShortName);
            }
            if (indexOfBookShortName == indexOfEndTitle - indexOfBookShortName)
                return "";
            if (indexOfEndTitle - indexOfBookShortName <= 0)
                return "";
            return data.Substring(indexOfBookShortName, indexOfEndTitle - indexOfBookShortName);
        }

        public static string RemoveWithInternal(this string data, string start, string end)
        {
            int lengthToMove = start.Length;

            int startIndex =
                data.IndexOf(start);

            if (startIndex == -1)
                return data;

            int endIndex =
                data.IndexOf(end, startIndex + start.Length);
            if (endIndex == -1)
                return data;
            endIndex += end.Length;
            return endIndex - startIndex > 0 ? data.Remove(startIndex, endIndex - startIndex) : data;
        }
    }
}
