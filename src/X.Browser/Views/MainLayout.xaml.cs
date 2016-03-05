using CoreLib.Sprites;
using System.Linq;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using X.Browser.ViewModels;

namespace X.Browser.Views
{
    public sealed partial class MainLayout : Page
    {
        ViewModels.BrowserVM vm = new ViewModels.BrowserVM();
        SpriteBatch _spriteBatch;
        private const string const_TabPreview = "tab-preview";

        public MainLayout()
        {
            this.InitializeComponent();

            this.DataContext = vm;
            header.InitChrome(App.Current, ApplicationView.GetForCurrentView());
            _spriteBatch = new SpriteBatch(canvasDXLayer);
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




        private void CanvasInvalidate()
        {
            _spriteBatch.Draw();
        }
    }
}
