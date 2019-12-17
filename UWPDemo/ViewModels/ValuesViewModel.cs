using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPDemo.Constants;
using UWPDemo.Helpers;
using UWPDemo.Interfaces;
using UWPDemo.Models;

namespace UWPDemo.ViewModels
{
    public class ValuesViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private readonly IEncryptionManager _encManager;
        private readonly ILocalStore _localStore;

        private int _oldStartIndex = -1;
        private int _newStartIndex = -1;

        public ValuesViewModel(IApiService apiService, IEncryptionManager encManager, ILocalStore localStore)
        {
            _apiService = apiService;
            _encManager = encManager;
            _localStore = localStore;
            ListValues.CollectionChanged += ListValues_CollectionChanged;
        }

        private async void ListValues_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _newStartIndex = e.NewStartingIndex;
                    if (_oldStartIndex != -1 && _newStartIndex != -1)
                    {
                        ListValues[_newStartIndex].Order = _newStartIndex;
                        ListValues[_oldStartIndex].Order = _oldStartIndex;

                        //reset
                        _oldStartIndex = -1;
                        _newStartIndex = -1;
                    }
                    await StoreData(ListValues);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    _oldStartIndex = e.OldStartingIndex;
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public ObservableRangeCollection<ValueModel> ListValues { get; set; } = new ObservableRangeCollection<ValueModel>();

        private bool _showLoading;
        public bool ShowLoading
        {
            get => _showLoading;
            set => Set(ref _showLoading, value);
        }
        public ICommand SyncCommand => new RelayCommand(async () => await DoSync());

        private async Task DoSync()
        {
            ShowLoading = true;
            if (_localStore.ContainsKey(StringConstants.LocalData))
            {
                //get values from secure storage
                var storedModels = await GetStoredData();
                ListValues.ReplaceRange(storedModels.OrderBy(x => x.Order));
            }
            else
            {
                var models = await _apiService.GetValueModelsAsync();
                ListValues.ReplaceRange(models.OrderBy(x => x.Order));

                //Store values securely
                await StoreData(models);
            }
            ShowLoading = false;
        }

        private async Task StoreData(IEnumerable<ValueModel> models)
        {
            string data = JsonSerializer.Serialize(models);
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            var securedData = await _encManager.EncryptV2(byteData).ConfigureAwait(false);
            _localStore.SaveData(StringConstants.LocalData, securedData);
        }

        private async Task<IEnumerable<ValueModel>> GetStoredData()
        {
            byte[] securedData = _localStore.GetData(StringConstants.LocalData, new byte[0]);
            var byteData = await _encManager.DecryptV2(securedData).ConfigureAwait(false);
            string data = Encoding.UTF8.GetString(byteData);
            return JsonSerializer.Deserialize<IEnumerable<ValueModel>>(data);
        }
    }
}
