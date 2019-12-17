using System.Threading.Tasks;

namespace UWPDemo.Interfaces
{
    public interface IEncryptionManager
    {
        Task<byte[]> DecryptV2(byte[] bytes);
        Task<byte[]> EncryptV2(byte[] bytes);
    }
}
