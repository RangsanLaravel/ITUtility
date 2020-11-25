using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace ITUtility
{
    public class SecurityHelper
    {
        public static string privateRefreshTokenKey = "S+fetc4qDmem/c2f7JuLtJWre2gUm5rCR/7oQKkk92g/NhK/MIRbBDpjbl8F5wZU9GC4tBhPCJvz+Zu/qlRrAw==";
        private static string privateTokenKey = "Tk6s37ZbrgDGNb8/LXtWcisCiRoKgus/9I3mK2lu4y0n9KBL+XLgSBnLelFxg+fSK7tPJonhKGNSrseKH6OIig==";
        private static string saltValueTokenKey = "VPd+ytAGDRegtAgbKeQxuIpykxwCldBwowu351QS/Gtu8WyMQ5Hhmuul+Bs6+Wjvv0I1pk38v4wFpm7tBCB+5g==";
        private static string APIKey = "3Yz50eMlNH0/GKtSzXZ5LdA6612A9cEGwWplX/svOv8=";
        private static string APISecretKey = "nxx1Ps7zrxjcN38GVD9+kEf/WAH0f+piyZ86pZioTok=";
        private static string saltValueAPIKey = "J2LqM1KSF42G3QtiyhsnxeQse97mZauaLfmVX/sbKTs=";

        private static string privateKeyForQueryString = "VTJJKXsENXyCfwpWaiEdXNMLbUDUUUqQ1bU8wmbwAvwaoGTCfk";
        private static string saltKeyForQueryString = "oSOZK4BOX6Nrkn+DZXkJLx5OKSfWtVWHd3";

        public static string CreateAPIKey(string value)
        {

            string plainText = APIKey;    // original plaintext
            string passPhrase = value;       // can be any string
            string saltValue = saltValueAPIKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 128;                // can be 192 or 128

            Console.WriteLine(String.Format("Plaintext : {0}", plainText));

            string cipherText = SecurityHelper.Encrypt
            (
                plainText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(cipherText));
        }

        public static string CreateAPISecretKey(string value)
        {

            string plainText = APISecretKey;    // original plaintext
            string passPhrase = value;       // can be any string
            string saltValue = saltValueAPIKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 128;                // can be 192 or 128

            Console.WriteLine(String.Format("Plaintext : {0}", plainText));

            string cipherText = SecurityHelper.Encrypt
            (
                plainText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(cipherText));
        }



        public static string CreateToken()
        {

            string plainText = privateTokenKey;    // original plaintext
            string passPhrase = Guid.NewGuid().ToString("N");       // can be any string
            string saltValue = saltValueTokenKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128

            Console.WriteLine(String.Format("Plaintext : {0}", plainText));

            string cipherText = SecurityHelper.Encrypt
            (
                plainText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(cipherText));
        }

        public static string CreateRefrshToken()
        {

            string plainText = privateRefreshTokenKey;    // original plaintext
            string passPhrase = Guid.NewGuid().ToString("N");        // can be any string
            string saltValue = saltValueTokenKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128

            // Console.WriteLine(String.Format("Plaintext : {0}", plainText));

            string cipherText = SecurityHelper.Encrypt
            (
                plainText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(cipherText));

        }

        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be encrypted.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be 
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Encrypted value formatted as a base64-encoded string.
        /// </returns>
        private static string Encrypt
        (
            string plainText,
            string passPhrase,
            string saltValue,
            string hashAlgorithm,
            int passwordIterations,
            string initVector,
            int keySize
        )
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations
            );

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor
            (
                keyBytes,
                initVectorBytes
            );

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                encryptor,
                CryptoStreamMode.Write
            );

            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="cipherText">
        /// Base64-formatted ciphertext value.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Decrypted string value.
        /// </returns>
        /// <remarks>
        /// Most of the logic in this function is similar to the Encrypt
        /// logic. In order for decryption to work, all parameters of this function
        /// - except cipherText value - must match the corresponding parameters of
        /// the Encrypt function which was called to generate the
        /// ciphertext.
        /// </remarks>
        private static string Decrypt
        (
            string cipherText,
            string passPhrase,
            string saltValue,
            string hashAlgorithm,
            int passwordIterations,
            string initVector,
            int keySize
        )
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations
            );

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor
            (
                keyBytes,
                initVectorBytes
            );

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                decryptor,
                CryptoStreamMode.Read
            );

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read
            (
                plainTextBytes,
                0,
                plainTextBytes.Length
            );

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString
            (
                plainTextBytes,
                0,
                decryptedByteCount
            );

            // Return decrypted string.   
            return plainText;
        }

        public static string Encrypt(string txt)
        {

            string plainText = txt;    // original plaintext
            string passPhrase = privateTokenKey;        // can be any string
            string saltValue = saltValueTokenKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128



            string cipherText = Encrypt
            (
                plainText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            return cipherText;

        }

        public static string Encrypt(string txt, string saltKey)
        {

            string plainText = txt;    // original plaintext
            string passPhrase = privateTokenKey;        // can be any string
            string saltValue = saltKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128



            string cipherText = Encrypt
            (
                plainText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            return cipherText;

        }

        public static string Decrypt(string cipherText)
        {
            string passPhrase = privateTokenKey;        // can be any string
            string saltValue = saltValueTokenKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128


            string plainText = Decrypt
            (
                cipherText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            return plainText;

        }

        public static string Decrypt(string cipherText, string saltKey)
        {
            string passPhrase = privateTokenKey;        // can be any string
            string saltValue = saltKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128


            string plainText = Decrypt
            (
                cipherText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            return plainText;

        }

        public static string RandomKey()
        {

            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[64];
            rng.GetBytes(tokenData);
            string token = Convert.ToBase64String(tokenData);
            return token;
        }


        public static bool IsValidAPIKey(string token)
        {
            string passPhrase = APIKey;        // can be any string
            string saltValue = saltValueAPIKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128

            try
            {
                string plainText = Decrypt
                (
                    token,
                    passPhrase,
                    saltValue,
                    hashAlgorithm,
                    passwordIterations,
                    initVector,
                    keySize
                );

                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool IsValidAPISecretKey(string token)
        {
            string passPhrase = APISecretKey;        // can be any string
            string saltValue = saltValueAPIKey;        // can be any string
            string hashAlgorithm = "SHA256";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "1ZENJFXPMai3HQ=="; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128

            try
            {
                string plainText = Decrypt
                (
                    token,
                    passPhrase,
                    saltValue,
                    hashAlgorithm,
                    passwordIterations,
                    initVector,
                    keySize
                );

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public static string GeneratToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }

        public static bool decodeToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            bool AUTHORIZE = false;
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when > DateTime.UtcNow.AddMinutes(-1))
            {
                AUTHORIZE = true;
            }
            return AUTHORIZE;
        }


   
    }
}
