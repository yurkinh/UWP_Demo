using System.Threading.Tasks;

namespace UWPDemo.Interfaces
{
    public interface IKeyManager
    {
        Task<string> GetEncryptionKey();
        Task SetEncryptionKey(string key);
        bool DeleteEncryptionKey();
        bool IsKeySet();
    }
}
