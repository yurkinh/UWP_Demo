using System.Threading.Tasks;
using UWPDemo.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel VM { get; set; }
        public MainPage()
        {
            InitializeComponent();
            VM = (MainViewModel)DataContext;
            GaugeAnimation.Begin();
        }

        private async void ButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            FlipAnimation.Begin();
            await Task.Delay(2000);
            App.Current.Exit();            
        }
        void DragableGridManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            DragableGrid.Opacity = 0.5;
        }

        void DragableGridManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            MyTransform.TranslateX += e.Delta.Translation.X;
            MyTransform.TranslateY += e.Delta.Translation.Y;
        }

        void DragableGridManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            DragableGrid.Opacity = 1;
        }

        private void PinFieldTextChanged(object sender, TextChangedEventArgs e)
        {
            NavigateButton.IsEnabled = !string.IsNullOrEmpty(PinField.Text) && PinField.Text.Length == 6;
        }
    }
}
