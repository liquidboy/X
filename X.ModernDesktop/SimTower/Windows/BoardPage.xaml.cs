using System;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace X.ModernDesktop.SimTower.Windows
{
  public sealed partial class BoardPage : Page
  {
    public BoardPage()
    {
      this.InitializeComponent();

      //ApplicationView.PreferredLaunchViewSize = new Size(600, 800);
      //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

    }

    //protected async override void OnNavigatedTo(NavigationEventArgs e)
    //{
    //  base.OnNavigatedTo(e);

    //  var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.Default);
    //  preferences.ViewSizePreference = ViewSizePreference.Custom;
    //  preferences.CustomSize = new Size { Width = 700, Height = 900 };

    //  ApplicationView.PreferredLaunchViewSize = new Size(600, 800);
    //  ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

    //  var view = ApplicationView.GetForCurrentView();
    //  await view.TryEnterViewModeAsync(ApplicationViewMode.Default, preferences);
    //}
  }
}
