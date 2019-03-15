using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AccountPortal.Domain.Extensions
{
    public class EncryptionUtility : IEncryptionUtility
    {
        //this is from stackoverflow, slightly modified
        public string Encrypt(string plainText)
        {
            const string encryptionKey = "NaCl";
            var clearBytes = Encoding.Unicode.GetBytes(plainText);
            using (var encryption = Aes.Create())
            {
                using (var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    encryption.Key = pdb.GetBytes(32);
                    encryption.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryption.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        plainText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return plainText;
        }
        public string Decrypt(string cipherText)
        {
            const string encryptionKey = "NaCl";
            cipherText = cipherText.Replace(" ", "+");
            var cipherBytes = Convert.FromBase64String(cipherText);
            using (var encryption = Aes.Create())
            {
                using (var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    encryption.Key = pdb.GetBytes(32);
                    encryption.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryption.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            return cipherText;
        }
    }
}