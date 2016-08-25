using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace X.Player
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        
        public void ProcessArguments(string arguments, string tileId)
        {
            //await ctlShellExt.InitExtensions();
            header.InitChrome(App.Current, ApplicationView.GetForCurrentView());
            //ctlViewer.Uri = "viewer://sketchflow-application.sketch";
            //ctlViewer.Uri = "viewer://flickr-application.flickr";
            ctlViewer.Uri = "viewer://windowsui-application.comp";
        }
    }
}
