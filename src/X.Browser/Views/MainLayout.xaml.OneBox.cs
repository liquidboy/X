using CoreLib.Extensions;
using CoreLib.Sprites;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using X.Browser.Messages;
using X.Browser.ViewModels;

namespace X.Browser.Views
{
    partial class MainLayout
    {

        private void InitOneBox()
        {

            Messenger.Default.Register<HideOnebox>(this, HideOnebox);
            Messenger.Default.Register<ShowOnebox>(this, ShowOnebox);

        }
        private void HideOnebox(HideOnebox message)
        {
            ShowHideOneBox(false);
        }

        private void ShowOnebox(ShowOnebox message)
        {
            ShowHideOneBox(true);
        }


        private void ShowHideOneBox(bool show)
        {
            if (show)
            {
                ctlShell.IsOneBoxHidden = false;
                ctlShell.DockedExtensionRight.Visibility = Visibility.Visible;
                ctlShell.DockedExtensionLeft.Visibility = Visibility.Visible;
                ctlShell.DockedExtensionBottomFull.Visibility = Visibility.Visible;
                ctlShell.DockedExtensionBottom.Visibility = Visibility.Visible;
            }
            else
            {
                ctlShell.IsOneBoxHidden = true;
                ctlShell.DockedExtensionRight.Visibility = Visibility.Collapsed;
                ctlShell.DockedExtensionLeft.Visibility = Visibility.Collapsed;
                ctlShell.DockedExtensionBottomFull.Visibility = Visibility.Collapsed;
                ctlShell.DockedExtensionBottom.Visibility = Visibility.Collapsed;
            }
        }

        private void ctrlModernUri_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ShowHideOneBox(true);
        }

    }
}
