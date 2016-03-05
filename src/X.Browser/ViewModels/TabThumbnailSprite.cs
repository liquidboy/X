using CoreLib.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace X.Browser.ViewModels
{
    public struct TabThumbnailSprite : IVisualTreeElement
    {
        public string ID { get; set; }
        public Rect Layout { get; set; }
        public bool IsVisible { get; set; }
        public string TextureBackgroundUri { get; set; }
        public bool CanDeleteFromTree { get; set; }



        UIElement _rootElement;
        UIElement _surface;

        public void Delete(ref UIElement surface)
        {
            if (!IsVisible)
            {
                DelayedDelete(60, surface, _rootElement);
            }
        }

        public void Draw(ref UIElement surface)
        {
            _surface = surface;

            Delete(ref surface);


            if (IsVisible && _rootElement == null)
            {
                var grd = new Grid();
                grd.SetValue(Canvas.LeftProperty, Layout.Left);
                grd.SetValue(Canvas.TopProperty, Layout.Top);
                grd.SetValue(Canvas.WidthProperty, Layout.Width);
                grd.SetValue(Canvas.HeightProperty, Layout.Height);
                grd.Opacity = 0;


                var rec = new Rectangle();
                rec.HorizontalAlignment = HorizontalAlignment.Stretch;
                rec.VerticalAlignment = VerticalAlignment.Stretch;
                rec.Fill = new SolidColorBrush(Color.FromArgb(255, 211, 211, 211));
                grd.Children.Add(rec);


                if (!string.IsNullOrEmpty(TextureBackgroundUri))
                {
                    var img = new Image();
                    img.HorizontalAlignment = HorizontalAlignment.Stretch;
                    img.VerticalAlignment = VerticalAlignment.Stretch;
                    img.Margin = new Thickness(10, 15, 10, 15);
                    img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(TextureBackgroundUri));
                    grd.Children.Add(img);
                }

                _rootElement = grd;

                var _this = this;
                grd.PointerExited += (o, e) => {
                    _this.DelayedDelete(100, _this._surface, _this._rootElement);
                };

                DelayedShow(20, surface);


                ((Canvas)surface).Children.Add(_rootElement);
            }


        }

        private void DelayedDelete(int millisecondsToDelay, UIElement surface, UIElement rootElement, int fadeDuration = 80)
        {

            if (rootElement != null)
            {

                //await Task.Delay(100);

                var sb = new Storyboard();
                sb.Duration = new Duration(TimeSpan.FromMilliseconds(millisecondsToDelay + fadeDuration));
                sb.BeginTime = TimeSpan.FromMilliseconds(millisecondsToDelay);
                var da = new DoubleAnimation();
                da.Duration = TimeSpan.FromMilliseconds(fadeDuration);
                da.From = 1;
                da.To = 0;
                da.AutoReverse = false;
                //da.EnableDependentAnimation = true;
                sb.Children.Add(da);

                Storyboard.SetTarget(da, rootElement);
                Storyboard.SetTargetProperty(da, "Opacity");

                var _local = this;
                sb.Completed += (s, e) => { ((Canvas)surface).Children.Remove(rootElement); _local.CanDeleteFromTree = true; };
                sb.Begin();


            }
        }

        private void DelayedShow(int millisecondsToDelay, UIElement surface, int fadeDuration = 15)
        {
            if (_rootElement != null)
            {
                var sb = new Storyboard();
                sb.Duration = new Duration(TimeSpan.FromMilliseconds(millisecondsToDelay + fadeDuration));
                sb.BeginTime = TimeSpan.FromMilliseconds(millisecondsToDelay);
                var da = new DoubleAnimation();
                da.Duration = TimeSpan.FromMilliseconds(fadeDuration);
                da.From = 0;
                da.To = 1;
                da.AutoReverse = false;
                //da.EnableDependentAnimation = true;
                sb.Children.Add(da);

                Storyboard.SetTarget(da, _rootElement);
                Storyboard.SetTargetProperty(da, "Opacity");

                sb.Begin();
            }
        }


    }
}
