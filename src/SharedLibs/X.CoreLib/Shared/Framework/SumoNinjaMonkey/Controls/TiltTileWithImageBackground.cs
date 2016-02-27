using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed class TiltTileWithImageBackground : Control
    {

        public event EventHandler Clicked;

        SplineDoubleKeyFrame kfX;
        SplineDoubleKeyFrame kfY;
        SplineDoubleKeyFrame kfScaleX;
        SplineDoubleKeyFrame kfScaleY;
        
        Grid mainGrid;
        Storyboard sbTilt;
        Storyboard sbHide;
        Storyboard sbShow;
        Storyboard sbExplode;
        Rectangle recCP;
        Rectangle recBackground;
        Rectangle recDisabled;
        TextBlock lblLabel;
        Image imgBackground;
        
        double maxTilt = 10;

        public Brush NormalBackground { get; set; }
        public Brush SelectedBackground { get; set; }
        public Brush DisabledBackground { get; set; }

        public enum eClickAction{
            Nothing,
            Explode,
            Hide,
            Glow
        }
        public eClickAction ClickAction { get; set; }

        private bool _IsDisabled;
        public bool IsDisabled { get { return _IsDisabled; } set { _IsDisabled = !value; if (recBackground != null) { recBackground.Fill = !value ? DisabledBackground : NormalBackground; } } }



        public string Label { get { return (string)GetValue(LabelProperty); } set { SetValue(LabelProperty, value); } }


        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(TiltTileWithImageBackground), new PropertyMetadata(string.Empty));

        

        public TiltTileWithImageBackground()
        {
            this.DefaultStyleKey = typeof(TiltTileWithImageBackground);

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (mainGrid == null)
            {

                mainGrid = (Grid)GetTemplateChild("grdTile");
                sbTilt = (Storyboard)mainGrid.Resources["sbTilt"];
                sbHide = (Storyboard)mainGrid.Resources["sbHide"];
                sbShow = (Storyboard)mainGrid.Resources["sbShow"];
                sbExplode = (Storyboard)mainGrid.Resources["sbExplode"];
                recCP = (Rectangle)GetTemplateChild("recCP");
                recBackground = (Rectangle)GetTemplateChild("recBackground");
                recDisabled = (Rectangle)GetTemplateChild("recDisabled");
                lblLabel = (TextBlock)GetTemplateChild("lblLabel");
                imgBackground = (Image)GetTemplateChild("imgBackground");

                DoubleAnimationUsingKeyFrames planeProjectionRotationX = (DoubleAnimationUsingKeyFrames)sbTilt.Children[0];
                DoubleAnimationUsingKeyFrames planeProjectionRotationY = (DoubleAnimationUsingKeyFrames)sbTilt.Children[1];
                DoubleAnimationUsingKeyFrames ScaleTransformX = (DoubleAnimationUsingKeyFrames)sbTilt.Children[2];
                DoubleAnimationUsingKeyFrames ScaleTransformY = (DoubleAnimationUsingKeyFrames)sbTilt.Children[3];
                kfX = (SplineDoubleKeyFrame)planeProjectionRotationX.KeyFrames[0];
                kfY = (SplineDoubleKeyFrame)planeProjectionRotationY.KeyFrames[0];
                kfScaleX = (SplineDoubleKeyFrame)ScaleTransformX.KeyFrames[0];
                kfScaleY = (SplineDoubleKeyFrame)ScaleTransformY.KeyFrames[0];

                mainGrid.PointerMoved += mainGrid_PointerMoved;
                mainGrid.PointerPressed +=mainGrid_PointerPressed;
                mainGrid.PointerReleased += mainGrid_PointerReleased;
                mainGrid.PointerExited += mainGrid_PointerExited;

                recBackground.Fill = NormalBackground;
                recDisabled.Fill = DisabledBackground;
                if (IsDisabled)
                {
                    recDisabled.Visibility = Visibility.Visible;
                    //mainGrid.Opacity = 0.4;
                }
                else
                {
                    recDisabled.Visibility = Visibility.Collapsed;
                    //mainGrid.Opacity = 1;
                }
            }
        }



        private void mainGrid_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            return;
            Point pt = e.GetCurrentPoint(recCP).RawPosition;

            TiltEffect(pt);
        }

        private void mainGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsDisabled) return;

            Point pt = e.GetCurrentPoint(recCP).RawPosition;
            recBackground.Fill = SelectedBackground;
            TiltEffect(pt);
        }

        private void mainGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (IsDisabled) return;

            kfX.Value = 0;
            kfY.Value = 0;
            kfScaleX.Value = 1.0;
            kfScaleY.Value = 1.0;
            recBackground.Fill = NormalBackground;
            sbTilt.Begin();

            if(ClickAction== eClickAction.Explode) Explode();

            if (Clicked != null) Clicked(this, EventArgs.Empty);
        }


        private void mainGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            kfX.Value = 0;
            kfY.Value = 0;
            kfScaleX.Value = 1.0;
            kfScaleY.Value = 1.0;
            recBackground.Fill = NormalBackground;
            sbTilt.Begin();
            sbTilt.Stop();
        }


        private void TiltEffect(Point pt)
        {

            kfX.Value = Math.Sign(pt.Y) * (Math.Abs(pt.Y) > maxTilt ? maxTilt : Math.Abs(pt.Y));
            kfY.Value = Math.Min(Math.Abs(pt.X), maxTilt) * Math.Sign(pt.X) * -1;
            kfScaleX.Value = 0.98;
            kfScaleY.Value = 0.98;
            sbTilt.Begin();
        }

        public void Hide()
        {
            sbHide.Begin();
        }

        public void Show()
        {
            sbShow.Begin();
        }

        public void Explode()
        {
            sbExplode.Begin();
        }

        public void LoadImage(Uri uri)
        {
            imgBackground.Opacity = 0.1;
            imgBackground.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(uri);
        }
    }
}
