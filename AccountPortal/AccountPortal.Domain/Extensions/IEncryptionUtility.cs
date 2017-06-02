
namespace AccountPortal.Domain.Extensions
{
    public interface IEncryptionUtility
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
        
    }
}
