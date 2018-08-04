using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using X.ModernDesktop.SimTower.Windows;

namespace X.ModernDesktop.SimTower.Views
{
  public sealed partial class StartMenu : UserControl
  {
    public StartMenu()
    {
      this.InitializeComponent();
    }

    private void butNewTower_Click(object sender, RoutedEventArgs e)
    {
      openWindow<BoardPage>();
    }

    private void butLoadTower_Click(object sender, RoutedEventArgs e)
    {

    }

    private async void openWindow<T>() {
      CoreApplicationView newView = CoreApplication.CreateNewView();
      int newViewId = 0;
      await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
      {
        Frame frame = new Frame();
        frame.Navigate(typeof(T), null);
        Window.Current.Content = frame;
        // You have to activate the window in order to show it later.
        Window.Current.Activate();

        newViewId = ApplicationView.GetForCurrentView().Id;
      });
      bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
    }
  }
}
