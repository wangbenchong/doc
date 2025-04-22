using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ScriptGenerator {
public abstract class Encryption {
    public static string Encrypt(string plainText, string email) {
        byte[] key, iv;
        GetKeyAndIv(email, out key, out iv);

        using var aes = new AesManaged { Key = key, IV = iv };
        using var encryptor = aes.CreateEncryptor();
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream)) {
            streamWriter.Write(plainText);
        }

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public static string Decrypt(string encryptedText, string email) {
        byte[] key, iv;
        GetKeyAndIv(email, out key, out iv);

        using var aes = new AesManaged { Key = key, IV = iv };
        using var decryptor = aes.CreateDecryptor();
        using var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText));
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return streamReader.ReadToEnd();
    }

    private static void GetKeyAndIv(string email, out byte[] key, out byte[] iv) {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(email));
        key = new byte[16];
        iv = new byte[16];
        Array.Copy(hash, 0, key, 0, 16);
        Array.Copy(hash, 16, iv, 0, 16);
    }
}
}