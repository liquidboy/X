using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace X.SharedLibs.Windows
{
  public class WindowService
  {
    //https://stackoverflow.com/questions/49204828/whats-the-correct-way-for-launching-a-new-view-with-a-specific-size-in-uwp
    public static async Task openWindow<T>(int width, int height)
    {
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
        //ApplicationView.GetForCurrentView().TryResizeView(new Size(width, height));
      });

      //var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
      //var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.Default);
      //preferences.ViewSizePreference = ViewSizePreference.Custom;
      //preferences.CustomSize = new Size { Width = width, Height = height };
      //bool viewShown = await ApplicationViewSwitcher.TryShowAsViewModeAsync(newViewId, ApplicationViewMode.CompactOverlay, preferences);

      bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId, ViewSizePreference.Custom);

    }

  }
}
