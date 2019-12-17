
namespace UWPDemo.Interfaces
{
    public interface ILocalStore
    {
        void SaveData(string key, byte[] value);
        byte[] GetData(string key, byte[] defaultValue);

        bool ContainsKey(string key);
        void RemoveData(string key);
    }
}
