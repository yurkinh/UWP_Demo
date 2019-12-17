using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MetroLog;
using UWPDemo.Constants;
using UWPDemo.Interfaces;
using UWPDemo.Services;
using UWPDemo.Views;

namespace UWPDemo.ViewModels
{
    public class ViewModelLocator
    {
        //shortcuts for viewmodels
        public MainViewModel MainPageVM => SimpleIoc.Default.GetInstance<MainViewModel>();
        public ValuesViewModel ValuesPageVM => SimpleIoc.Default.GetInstance<ValuesViewModel>();

        public void Init()
        {

        }

        static ViewModelLocator()
        {
            RegisterServices();
            RegisterViewModels();
        }

        private static void RegisterServices()
        {            
            SimpleIoc.Default.Register(() => CreateNavigationService());
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IApiService, MockApiService>();
            SimpleIoc.Default.Register<IKeyManager, KeyManager>();
            SimpleIoc.Default.Register<IEncryptionManager, EncryptionManager>();
            SimpleIoc.Default.Register<ILocalStore, LocalStore>();
            SimpleIoc.Default.Register<ILoggingService, LoggingService>();
        }

        private static void RegisterViewModels()
        {
            // Registration of the viewmodels
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ValuesViewModel>();            
        }

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();

            // New pages
            navigationService.Configure(StringConstants.MainPage, typeof(MainPage));
            navigationService.Configure(StringConstants.ValuesPage, typeof(ValuesPage));

            return navigationService;
        }
    }
}
