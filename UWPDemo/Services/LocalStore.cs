using UWPDemo.Constants;
using UWPDemo.Interfaces;
using Windows.Storage;

namespace UWPDemo.Services
{
    public class LocalStore : ILocalStore
    {
        static readonly object locker = new object();
        public byte[] GetData(string key, byte[] defaultValue)
        {
            lock (locker)
            {
                var appDataContainer = GetApplicationDataContainer(StringConstants.LocalStorage);
                if (appDataContainer.Values.ContainsKey(key))
                {
                    var tempValue = appDataContainer.Values[key];
                    if (tempValue != null)
                        return (byte[])tempValue;
                }
            }

            return defaultValue;
        }

        public void SaveData(string key, byte[] value)
        {
            lock (locker)
            {
                var appDataContainer = GetApplicationDataContainer(StringConstants.LocalStorage);

                if (value == null)
                {
                    if (appDataContainer.Values.ContainsKey(key))
                        appDataContainer.Values.Remove(key);
                    return;
                }

                appDataContainer.Values[key] = value;
            }
        }        

        public bool ContainsKey(string key)
        {
            lock (locker)
            {
                var appDataContainer = GetApplicationDataContainer(StringConstants.LocalStorage);
                return appDataContainer.Values.ContainsKey(key);
            }
        }

        public void RemoveData(string key)
        {
            lock (locker)
            {
                var appDataContainer = GetApplicationDataContainer(StringConstants.LocalStorage);
                if (appDataContainer.Values.ContainsKey(key))
                    appDataContainer.Values.Remove(key);
            }
        }

        static ApplicationDataContainer GetApplicationDataContainer(string sharedName)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (string.IsNullOrWhiteSpace(sharedName))
                return localSettings;

            if (!localSettings.Containers.ContainsKey(sharedName))
                localSettings.CreateContainer(sharedName, ApplicationDataCreateDisposition.Always);

            return localSettings.Containers[sharedName];
        }
    }
}
