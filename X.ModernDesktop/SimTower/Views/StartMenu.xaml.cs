using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using X.ModernDesktop.SimTower.Windows;
using X.SharedLibs.Windows;

namespace X.ModernDesktop.SimTower.Views
{
  public sealed partial class StartMenu : UserControl
  {
    public StartMenu()
    {
      this.InitializeComponent();
    }

    private async void butNewTower_Click(object sender, RoutedEventArgs e)
    {
      await WindowService.openWindow<BoardPage>(550,700);
      await WindowService.openWindow<CockpitViewPage>(300,400);
    }

    private void butLoadTower_Click(object sender, RoutedEventArgs e)
    {

    }
    
  }
}
