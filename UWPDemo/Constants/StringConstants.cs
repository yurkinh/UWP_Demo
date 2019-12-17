using Windows.ApplicationModel;

namespace UWPDemo.Constants
{
    public static class StringConstants
    {
        public const string PinKey = "Pin";
        public static readonly string SecureStorage = $"{Package.Current.Id}.securestorage";
        public static readonly string LocalStorage = $"{Package.Current.Id}.localstorage";
        public const string MainPage = "MainPageView";
        public const string ValuesPage = "ValuesPageView";
        public const string ConfirmPlaceholder = "Please confirm your pin";
        public const string DefaultPlaceholder = "123456";
        public const string LocalData = "LocalData";
        public const string PinErrorMessage = "Please provide correct pin";
        public const string PinConfirmation = "Pin confirmation";
        public const string OK = "Ok";
    }
}
