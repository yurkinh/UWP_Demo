using UWPDemo.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPDemo.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ValuesPage : Page
    {
        public ValuesViewModel VM { get; set; }
        public ValuesPage()
        {
            InitializeComponent();
            VM = (ValuesViewModel)DataContext;
        }
    }
}
