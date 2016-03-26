using CoreLib.Extensions;
using CoreLib.Sprites;
using System.Linq;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using X.Browser.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace X.Browser.Views
{
    public sealed partial class MainLayout : Page, IExtension
    {
        //ViewModels.BrowserVM vm = new ViewModels.BrowserVM();
        ViewModels.BrowserVM vm;
        ViewModels.AddTabVM atvm = new AddTabVM();

        public MainLayout()
        {
            this.InitializeComponent();
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


        public void ProcessArguments(string arguments, string tileId) {

            if (arguments.Contains("TabWasPinnedAt")) SetSelectedTab(tileId);
            else {

                if (arguments.Contains("DefaultTabTo") && tileId != "App") vm = new ViewModels.BrowserVM(defaultTabUid: tileId);
                else vm = new ViewModels.BrowserVM(); 

                this.DataContext = vm;
                AddTab.DataContext = atvm;
                header.InitChrome(App.Current, ApplicationView.GetForCurrentView());
                InitOneBox();
                InitTabs();
                InitExtensions();
            }



        }

    }
}
