using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UWPDemo.Constants;
using UWPDemo.Interfaces;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;

namespace UWPDemo.Services
{
    public class KeyManager: IKeyManager
    {
        public async Task<string> GetEncryptionKey()
        {
            var settings = GetSettings(StringConstants.SecureStorage);

            var encBytes = settings.Values[StringConstants.PinKey] as byte[];

            if (encBytes == null)
                return null;

            var provider = new DataProtectionProvider();

            var buffer = await provider.UnprotectAsync(encBytes.AsBuffer());

            return Encoding.UTF8.GetString(buffer.ToArray());
        }

        public async Task SetEncryptionKey(string key)
        {
            var settings = GetSettings(StringConstants.SecureStorage);

            var bytes = Encoding.UTF8.GetBytes(key);

            // LOCAL=user and LOCAL=machine do not require enterprise auth capability
            var provider = new DataProtectionProvider("LOCAL=user");

            var buffer = await provider.ProtectAsync(bytes.AsBuffer());

            var encBytes = buffer.ToArray();

            settings.Values[StringConstants.PinKey] = encBytes;
        }

        public bool DeleteEncryptionKey()
        {
            var settings = GetSettings(StringConstants.SecureStorage);

            if (settings.Values.ContainsKey(StringConstants.PinKey))
            {
                settings.Values.Remove(StringConstants.PinKey);
                return true;
            }

            return false;
        }

        public bool IsKeySet()
        {
            var settings = GetSettings(StringConstants.SecureStorage);

            return settings.Values.ContainsKey(StringConstants.PinKey);
        }

        static ApplicationDataContainer GetSettings(string name)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            if (!localSettings.Containers.ContainsKey(name))
                localSettings.CreateContainer(name, ApplicationDataCreateDisposition.Always);
            return localSettings.Containers[name];
        }
        
    }
}
