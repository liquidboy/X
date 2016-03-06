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
       

        public MainLayout()
        {
            this.InitializeComponent();

            this.DataContext = vm;
            header.InitChrome(App.Current, ApplicationView.GetForCurrentView());
            InitTabs();
            InitExtensions();
        }

       
    }
}
