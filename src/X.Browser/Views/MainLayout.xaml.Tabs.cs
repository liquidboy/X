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

        SpriteBatch _spriteBatch;
        private const string const_TabPreview = "tab-preview";


        private void InitTabs() {
            _spriteBatch = new SpriteBatch(canvasDXLayer);
            Messenger.Default.Register<SetAddTabSearchBoxFocus>(this, SetAddTabSearchBoxFocus);
            //Messenger.Default.Register<CloseAddTabFlyout>(this, CloseAddTabFlyout);
        }

        private void tlMainTabs_TabPointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TabViewModel tvm = null;

            if (sender is FrameworkElement)
            {
                var fe = (FrameworkElement)sender;
                if (fe.DataContext is TabViewModel) tvm = (TabViewModel)fe.DataContext;
                else return;


                var visual = fe.TransformToVisual(layoutRoot);
                var point1 = visual.TransformPoint(new Point(0, 40));
                var point2 = new Point(point1.X + fe.ActualWidth + 180, point1.Y + fe.ActualHeight + 140);

                //hide all the current tabs in the canvas
                _spriteBatch.Elements.ToList().ForEach(delegate (IVisualTreeElement element) { element.IsVisible = false; });

                //now delete all the relevant elements in the spritebatch
                _spriteBatch.DeleteAll();

                //create the new thumbnail sprite for current button
                _spriteBatch.Add(new TabThumbnailSprite() { Layout = new Rect(point1, point2), ID = const_TabPreview, TextureBackgroundUri = tvm.ThumbUri, IsVisible = true });

                _spriteBatch.IsVisible = true;

            }


            CanvasInvalidate();
        }

        private void tlMainTabs_TabPointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _spriteBatch.Elements.ToList().ForEach(delegate (IVisualTreeElement element) { element.IsVisible = false; });
            CanvasInvalidate();
        }

        private void SetAddTabSearchBoxFocus(SetAddTabSearchBoxFocus message)
        {
            AddTab.FocusOnSearchBox();

            var vm = (AddTabVM)AddTab.DataContext;
            vm.SearchCommand.Execute("http://www.msn.com/spartan/ntp?dpir=1");

        }

        private void CanvasInvalidate()
        {
            _spriteBatch.Draw();
        }

        private void AddTab_LoadCompleted(object sender, System.EventArgs e)
        {
            if (e is UI.AddTab.LoadCompletedEventArgs)
            {
                //var vm = (ViewModel.AddTabViewModel)((FrameworkElement)sender).DataContext;
                //vm.SearchQuery = ((LoadCompletedEventArgs)e).SearchQuery;

            }
        }

        private void AddTab_DoSearch(object sender, System.EventArgs e)
        {
            if (e is UI.AddTab.DoSearchEventArgs)
            {
                //var vm = (ViewModel.AddTabViewModel)((FrameworkElement)sender).DataContext;
                //vm.SearchCommand.Execute(((DoSearchEventArgs)e).SearchQuery);
            }
        }

        public void SetSelectedTab(string tileId) {

            var found = vm.Tabs.Where(x => x.Uid == tileId).First();
            if (found != null) {
                vm.SelectedTab = found;
                vm.ExposedNotifyPropertyChanged("SelectedTab");
            }
            
        }
    }
}
