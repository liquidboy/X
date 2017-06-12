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
using X.NeonShell.Extensions;

namespace X.NeonShell.Features.HamburgerNavigation
{
    public sealed partial class HamburgerNavigationView : UserControl
    {
        public HamburgerNavigationView()
        {
            this.InitializeComponent();
        }

        private void NavRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            OnNavigationChanged?.Invoke(null, new NavigationChangedArgs() { Data = (string)rb.Tag, FriendlyText = (string)rb.Content });
        }



        public List<Scenario> TreeItems
        {
            get { return (List<Scenario>)GetValue(TreeItemsProperty); }
            set { SetValue(TreeItemsProperty, value); }
        }

        public static readonly DependencyProperty TreeItemsProperty =
            DependencyProperty.Register("TreeItems", typeof(List<Scenario>), typeof(HamburgerNavigationView), new PropertyMetadata(null));




        public void SetSelectedMenuItem(string contentName) {

            foreach (var rb in this.FindVisualChildList<RadioButton>())
            {
                if (contentName == ((Scenario)rb.DataContext).ClassType.Name)
                {
                    rb.IsChecked = true;
                    return;
                }
            }
        }
        
        public event EventHandler<NavigationChangedArgs> OnNavigationChanged;


        
    }

    //public class TagBindingConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, string language)
    //    {
    //        return System.Net.WebUtility.HtmlDecode((string)value);
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        return true;
    //    }
    //}

    public class NavigationChangedArgs : EventArgs {
        public string Data;
        public string FriendlyText;
    }
}
