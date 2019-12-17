using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPDemo.Constants;
using UWPDemo.Interfaces;

namespace UWPDemo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IKeyManager _keyManager;
        private string _pin;

        public MainViewModel(INavigationService navigationService, IKeyManager keyManager, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _keyManager = keyManager;

            //Check is pin already setup
            CheckPin();
        }

        private string _placeHolder;
        public string Placeholder
        {
            get => _placeHolder;
            set => Set(ref _placeHolder, value);
        }

        public bool IsPinSet { get; set; }

        //TODO If no passcode is set in the vault, the user can enter one and will then be navigated to the DetailPageView
        public ICommand SetPasswordAndNavigateCommand => new RelayCommand<string>(async (pas) => await SetPasswordAndNavigate(pas));

        private async Task SetPasswordAndNavigate(string password)
        {
            await _keyManager.SetEncryptionKey(password);

            _navigationService.NavigateTo(StringConstants.ValuesPage);
        }

        //TODO If a passcode has already been stored, use this to validate and navigate
        public ICommand ValidatePasswordAndNavigateCommand => new RelayCommand<string>(async (pas) => await ValidatePasswordAndNavigate(pas));

        private async Task ValidatePasswordAndNavigate(string password)
        {
            if (string.IsNullOrEmpty(_pin))
            {
                _pin = await _keyManager.GetEncryptionKey();
            }

            if (_pin == password)
            {
                _navigationService.NavigateTo(StringConstants.ValuesPage);
            }
            else
            {
                await _dialogService.ShowError(StringConstants.PinErrorMessage, StringConstants.PinConfirmation, StringConstants.OK, null);
            }
        }

        public ICommand NavigateButtonCommand => new RelayCommand<string>(DoNavigate);

        private void DoNavigate(string password)
        {
            if (IsPinSet)
            {
                ValidatePasswordAndNavigateCommand.Execute(password);
            }
            else
            {
                SetPasswordAndNavigateCommand.Execute(password);
            }
        }

        private void CheckPin()
        {
            IsPinSet = _keyManager.IsKeySet();
            Placeholder = IsPinSet ? StringConstants.ConfirmPlaceholder : StringConstants.DefaultPlaceholder;
        }
    }
}
