using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;
/// <summary>
/// Summary description for Crypto
/// </summary>
static public class Crypto
{

    private static readonly byte[] salt = Encoding.ASCII.GetBytes("Ent3r your oWn S@lt v@lu# h#r3");
    public static string Encrypt(string textToEncrypt, string encryptionPassword)
    {
        var algorithm = GetAlgorithm(encryptionPassword);

        byte[] encryptedBytes;
        using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
            encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
        }
        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string encryptedText, string encryptionPassword)
    {
        var algorithm = GetAlgorithm(encryptionPassword);

        byte[] descryptedBytes;
        using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
        }
        return Encoding.UTF8.GetString(descryptedBytes);
    }

    private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
    {
        MemoryStream memory = new MemoryStream();
        using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
        {
            stream.Write(data, 0, data.Length);
        }
        return memory.ToArray();
    }
    private static RijndaelManaged GetAlgorithm(string encryptionPassword)
    {
        var key = new Rfc2898DeriveBytes(encryptionPassword, salt);
        var algorithm = new RijndaelManaged();
        int bytesForKey = algorithm.KeySize / 8;
        int bytesForIV = algorithm.BlockSize / 8;
        algorithm.Key = key.GetBytes(bytesForKey);
        algorithm.IV = key.GetBytes(bytesForIV);
        return algorithm;
    }
    public static string EncryptData(string Password)
    {
        string encryptionPassword = "supersecret";
        string encrypted = Crypto.Encrypt(Password, encryptionPassword);
        return encrypted;
    }
    public static string DecryptData(string Password)
    {
        string encryptionPassword = "supersecret";
        string original = Crypto.Decrypt(Password, encryptionPassword);
        return original;
    }

}