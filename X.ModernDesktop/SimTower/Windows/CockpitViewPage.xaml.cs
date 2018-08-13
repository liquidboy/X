using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.ModernDesktop.SimTower.Windows
{

  public sealed partial class CockpitViewPage : Page
  {
    public CockpitViewPage()
    {
      this.InitializeComponent();

      //ApplicationView.PreferredLaunchViewSize = new Size(500, 500);
      //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
    }

    private void lbToolbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var lbi = (ListBoxItem)e.AddedItems[0];
      var tool = (string)lbi.Content;

      App.windowsEventAggregator.Publish<string>(tool);
    }
  }
}
