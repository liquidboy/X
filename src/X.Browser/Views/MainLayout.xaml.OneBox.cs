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
                spOnebox.Visibility = Visibility.Visible;
                spRightToolbar.Visibility = Visibility.Visible;
                grdDockedExtensionRight.Visibility = Visibility.Visible;
                grdDockedExtensionLeft.Visibility = Visibility.Visible;
                grdDockedExtensionBottomFull.Visibility = Visibility.Visible;
                grdDockedExtensionBottom.Visibility = Visibility.Visible;
            }
            else
            {
                spOnebox.Visibility = Visibility.Collapsed;
                spRightToolbar.Visibility = Visibility.Collapsed;
                grdDockedExtensionRight.Visibility = Visibility.Collapsed;
                grdDockedExtensionLeft.Visibility = Visibility.Collapsed;
                grdDockedExtensionBottomFull.Visibility = Visibility.Collapsed;
                grdDockedExtensionBottom.Visibility = Visibility.Collapsed;
            }
        }

        private void ctrlModernUri_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ShowHideOneBox(true);
        }

    }
}
