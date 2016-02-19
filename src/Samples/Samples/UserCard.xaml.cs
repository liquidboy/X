using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Samples
{
    public sealed partial class UserCard : UserControl
    {
        public UserCard()
        {
            this.InitializeComponent();

            Loaded += UserCard_Loaded;
            Unloaded += UserCard_Unloaded;


        }

        private void UserCard_Unloaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UserCard_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            bkgLayer.DrawUIElements(root);  //will draw at index 0 (RenderTargetIndexFor_icTabList)
            bkgLayer.InitLayer(root.ActualWidth, root.ActualHeight, 0, 0);
        }


     
    }
}
