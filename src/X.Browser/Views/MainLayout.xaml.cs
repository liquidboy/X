using CoreLib.Extensions;
using CoreLib.Sprites;
using System.Linq;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using X.Browser.ViewModels;

namespace X.Browser.Views
{
    public sealed partial class MainLayout : Page, IExtension
    {
        ViewModels.BrowserVM vm = new ViewModels.BrowserVM();
        ViewModels.AddTabVM atvm = new AddTabVM();

        public MainLayout()
        {
            this.InitializeComponent();

            this.DataContext = vm;
            AddTab.DataContext = atvm;
            header.InitChrome(App.Current, ApplicationView.GetForCurrentView());
            InitOneBox();
            InitTabs();
            InitExtensions();
        }


        //chrome
        private void grid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            header.EnableResizeFix = false;
        }

        private void header_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            header.EnableResizeFix = true;
        }



    }
}
