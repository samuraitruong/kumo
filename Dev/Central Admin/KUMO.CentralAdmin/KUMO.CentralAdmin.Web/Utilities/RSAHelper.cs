using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KUMO.CentralAdmin.Web.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;

    namespace Extensions
    {
        public static class Extensions
        {
            /// <summary>
            /// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
            /// </summary>
            /// <param name="stringToEncrypt">String that must be encrypted.</param>
            /// <param name="key">Encryptionkey.</param>
            /// <returns>A string representing a byte array separated by a minus sign.</returns>
            /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
            public static string Encrypt(this string stringToEncrypt, string key)
            {
                if (string.IsNullOrEmpty(stringToEncrypt))
                {
                    throw new ArgumentException("An empty string value cannot be encrypted.");
                }

                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
                }

                CspParameters cspp = new CspParameters();
                cspp.KeyContainerName = key;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
                rsa.PersistKeyInCsp = true;

                byte[] bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(stringToEncrypt), true);

                return BitConverter.ToString(bytes);
            }

            /// <summary>
            /// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
            /// </summary>
            /// <param name="stringToDecrypt">String that must be decrypted.</param>
            /// <param name="key">Decryptionkey.</param>
            /// <returns>The decrypted string or null if decryption failed.</returns>
            /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
            public static string Decrypt(this string stringToDecrypt, string key)
            {
                string result = null;

                if (string.IsNullOrEmpty(stringToDecrypt))
                {
                    throw new ArgumentException("An empty string value cannot be encrypted.");
                }

                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
                }

                try
                {
                    CspParameters cspp = new CspParameters();
                    cspp.KeyContainerName = key;

                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
                    rsa.PersistKeyInCsp = true;

                    string[] decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
                    byte[] decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));


                    byte[] bytes = rsa.Decrypt(decryptByteArray, true);

                    result = System.Text.UTF8Encoding.UTF8.GetString(bytes);

                }
                finally
                {
                    // no need for further processing
                }

                return result;
            }


        }
    }

    public class RSAHelper
    {

      
        public string EncryptString(string inputString, int dwKeySize, string xmlString)
        {
            // TODO: Add Proper Exception Handlers
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize);
            rsaCryptoServiceProvider.FromXmlString(xmlString);
            int keySize = dwKeySize / 8;
            byte[] bytes = Encoding.UTF32.GetBytes(inputString);
            // The hash function in use by the .NET RSACryptoServiceProvider here is SHA1
            // int maxLength = ( keySize ) - 2 - ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
            int maxLength = keySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, true);
                // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after encryption and before decryption.
                // If you do not require compatibility with Microsoft Cryptographic API (CAPI) and/or other vendors.
                // Comment out the next line and the corresponding one in the DecryptString function.
                Array.Reverse(encryptedBytes);
                // Why convert to base 64?
                // Because it is the largest power-of-two base printable using only ASCII characters
                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }
            return stringBuilder.ToString();
        }

        public string DecryptString(string inputString, int dwKeySize, string xmlString)
        {
            // TODO: Add Proper Exception Handlers
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize);
            rsaCryptoServiceProvider.FromXmlString(xmlString);
            int base64BlockSize = ((dwKeySize / 8) % 3 != 0) ? (((dwKeySize / 8) / 3) * 4) + 4 : ((dwKeySize / 8) / 3) * 4;
            int iterations = inputString.Length / base64BlockSize;
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < iterations; i++)
            {
                byte[] encryptedBytes = Convert.FromBase64String(inputString.Substring(base64BlockSize * i, base64BlockSize));
                // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after encryption and before decryption.
                // If you do not require compatibility with Microsoft Cryptographic API (CAPI) and/or other vendors.
                // Comment out the next line and the corresponding one in the EncryptString function.
                Array.Reverse(encryptedBytes);
                arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
            }
            return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
        }

    }
}