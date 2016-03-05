using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace X.Browser.Views
{
    public sealed partial class MainLayout : Page
    {
        ViewModels.BrowserVM vm = new ViewModels.BrowserVM();

        public MainLayout()
        {
            this.InitializeComponent();

            this.DataContext = vm;
            header.InitChrome(App.Current, ApplicationView.GetForCurrentView());
        }

        private void tlMainTabs_TabPointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void tlMainTabs_TabPointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }
    }
}
