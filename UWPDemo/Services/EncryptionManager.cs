using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using UWPDemo.Interfaces;
using MetroLog;

namespace UWPDemo.Services
{
    public class EncryptionManager : IEncryptionManager
    {
        private readonly IKeyManager _keyManager;
        private readonly ILoggingService _loggingService;
        public EncryptionManager(IKeyManager keyManager, ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _keyManager = keyManager;            
        }

        public async Task<byte[]> EncryptV2(byte[] bytes)
        {
            try
            {
                var key = await _keyManager.GetEncryptionKey();

                return EncryptDataV2(bytes, key, key);
            }
            catch (Exception ex)
            {                
                _loggingService.WriteLine<App>(ex.Message, LogLevel.Error, ex);
                return new byte[0];
            }
        }

        private byte[] EncryptDataV2(byte[] bytes, string pw, string salt)
        {
            var base64Text = Convert.ToBase64String(bytes);

            var pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
            var saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
            var plainBuffer = CryptographicBuffer.ConvertStringToBinary(base64Text, BinaryStringEncoding.Utf16LE);


            // Derive key material for password size 32 bytes for AES256 algorithm
            var keyDerivationProvider = KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha1);
            // using salt and 1000 iterations
            var pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

            // create a key based on original key and derivation parmaters
            var keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
            var keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
            var derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

            // derive buffer to be used for encryption salt from derived password key 
            var saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

            // display the buffers – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
            //var keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
            //var saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

            var symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            // create symmetric key from derived password key
            var symmKey = symProvider.CreateSymmetricKey(keyMaterial);

            // encrypt data buffer using symmetric key and derived salt material
            var resultBuffer = CryptographicEngine.Encrypt(symmKey, plainBuffer, saltMaterial);
            byte[] result;
            CryptographicBuffer.CopyToByteArray(resultBuffer, out result);
           
            return result;
        }

        private byte[] DecryptDataV2(byte[] encryptedData, string pw, string salt)
        {            
            try
            {
                var pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
                var saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
                var cipherBuffer = CryptographicBuffer.CreateFromByteArray(encryptedData);

                // Derive key material for password size 32 bytes for AES256 algorithm
                var keyDerivationProvider =
                    KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithmNames.Pbkdf2Sha1);
                // using salt and 1000 iterations
                var pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);


                // create a key based on original key and derivation parmaters
                var keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
                var keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
                var derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

                // derive buffer to be used for encryption salt from derived password key 
                var saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

                // display the keys – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
                //var keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
                //var saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

                var symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
                
                var symmKey = symProvider.CreateSymmetricKey(keyMaterial);

                
                var resultBuffer = CryptographicEngine.Decrypt(symmKey, cipherBuffer, saltMaterial);
               
                var result = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, resultBuffer);
               
                var byteArray2 = Convert.FromBase64String(result);

               
                return byteArray2;
            }
            catch (Exception ex)
            {                
                _loggingService.WriteLine<App>(ex.Message, LogLevel.Error, ex);
                return new byte[0];
            }            
        }

        public async Task<byte[]> DecryptV2(byte[] bytes)
        {
            try
            {
                if (bytes == null) return null;
                var key = await _keyManager.GetEncryptionKey();                

                return DecryptDataV2(bytes, key, key);
            }
            catch (Exception ex)
            {
                var mess = ex.Message;
                _loggingService.WriteLine<App>(ex.Message, LogLevel.Error, ex);
                return new byte[0];
            }
        }       
    }
}