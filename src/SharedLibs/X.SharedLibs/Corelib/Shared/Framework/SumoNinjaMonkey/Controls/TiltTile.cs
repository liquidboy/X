using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed class TiltTile : ButtonBase //Control
    {
        public event PointerEventHandler Clicked;

        SplineDoubleKeyFrame kfX;
        SplineDoubleKeyFrame kfY;
        SplineDoubleKeyFrame kfScaleX;
        SplineDoubleKeyFrame kfScaleY;
        
        Grid mainGrid;
        Storyboard sbTilt;
        Storyboard sbHide;
        Storyboard sbShow;
        Storyboard sbExplode;
        Storyboard sbShowImage;
        Rectangle recCP;
        Rectangle recBackground;
        Rectangle recDisabled;
        Rectangle recSelected;
        TextBlock lblLabel;
        Image imgBackground;
        ContentControl ccIcon;
        ContentControl ccContent;
        Path pthMoreItems;

        double maxTilt = 10;

        public Brush NormalBackground { get; set; }
        public Brush SelectedBackground { get; set; }
        public Brush DisabledBackground { get; set; }






        public UIElement ContentControl
        {
            get { return (UIElement)GetValue(ContentControlProperty); }
            set { SetValue(ContentControlProperty, value); }
        }

        public static readonly DependencyProperty ContentControlProperty =
            DependencyProperty.Register("ContentControl", typeof(UIElement), typeof(TiltTile), new PropertyMetadata(null));

        
        


        DispatcherTimer dtResetPressedState;

        public enum eClickAction{
            Nothing,
            Explode,
            Hide,
            Glow,
            Messenger
        }
        
        public eClickAction ClickAction { get; set; }
        public string ClickMessengerIdentifier { get; set; }
        public string ClickMessengerAction { get; set; }
        public string ClickMessengerAggregateId { get; set; } 

        private bool _IsPressed = false;

        public bool PointerReleasedHandled { get; set; }

        private bool _IsDisabled;
        public bool IsDisabled { get { return _IsDisabled; } set { _IsDisabled = !value; if (recBackground != null) { recBackground.Fill = !value ? DisabledBackground : NormalBackground; } } }

        private bool _MoreItemsIconIsVisible;
        public bool MoreItemsIconIsVisible
        {
            get { return _MoreItemsIconIsVisible; }
            set
            {
                _MoreItemsIconIsVisible = value;

                if (pthMoreItems == null) return; 
                if (_MoreItemsIconIsVisible) pthMoreItems.Visibility = Windows.UI.Xaml.Visibility.Visible;
                else pthMoreItems.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;

                if (IsSelected)
                {
                    recSelected.Visibility = Visibility.Visible;
                }
                else
                {
                    recSelected.Visibility = Visibility.Collapsed;
                }

            }
        }


        public string Label { get { return (string)GetValue(LabelProperty); } set { SetValue(LabelProperty, value); } }


        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(TiltTile), new PropertyMetadata(string.Empty));


        public double LabelFontSize { get { return (double)GetValue(LabelFontSizeProperty); } set { SetValue(LabelFontSizeProperty, value); } }


        public static readonly DependencyProperty LabelFontSizeProperty =
            DependencyProperty.Register("LabelFontSize", typeof(double), typeof(TiltTile), new PropertyMetadata(28));



        public TiltTile()
        {
            this.DefaultStyleKey = typeof(TiltTile);
            dtResetPressedState = new DispatcherTimer();
            dtResetPressedState.Interval = TimeSpan.FromSeconds(2);
            dtResetPressedState.Tick += (sender, e) => { _IsPressed = false; dtResetPressedState.Stop(); _doRelease(null); };

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
                sbShowImage = (Storyboard)mainGrid.Resources["sbShowImage"];
                recCP = (Rectangle)GetTemplateChild("recCP");
                recBackground = (Rectangle)GetTemplateChild("recBackground");
                recDisabled = (Rectangle)GetTemplateChild("recDisabled");
                recSelected = (Rectangle)GetTemplateChild("recSelected");
                lblLabel = (TextBlock)GetTemplateChild("lblLabel");
                imgBackground = (Image)GetTemplateChild("imgBackground");
                ccIcon = (ContentControl)GetTemplateChild("ccIcon");
                ccContent = (ContentControl)GetTemplateChild("ccContent");
                pthMoreItems = (Path)GetTemplateChild("pthMoreItems");

                DoubleAnimationUsingKeyFrames planeProjectionRotationX = (DoubleAnimationUsingKeyFrames)sbTilt.Children[0];
                DoubleAnimationUsingKeyFrames planeProjectionRotationY = (DoubleAnimationUsingKeyFrames)sbTilt.Children[1];
                DoubleAnimationUsingKeyFrames ScaleTransformX = (DoubleAnimationUsingKeyFrames)sbTilt.Children[2];
                DoubleAnimationUsingKeyFrames ScaleTransformY = (DoubleAnimationUsingKeyFrames)sbTilt.Children[3];
                kfX = (SplineDoubleKeyFrame)planeProjectionRotationX.KeyFrames[0];
                kfY = (SplineDoubleKeyFrame)planeProjectionRotationY.KeyFrames[0];
                kfScaleX = (SplineDoubleKeyFrame)ScaleTransformX.KeyFrames[0];
                kfScaleY = (SplineDoubleKeyFrame)ScaleTransformY.KeyFrames[0];

                
                mainGrid.PointerMoved += mainGrid_PointerMoved;
                mainGrid.PointerPressed += mainGrid_PointerPressed;
                this.Click += TiltTile_Click; //this is what is triggered by buttonbase , pointerreleased is no longer triggered
                mainGrid.PointerReleased += mainGrid_PointerReleased; //may not need this since moving to ButtonBase
                mainGrid.PointerExited += mainGrid_PointerExited;


                recBackground.Fill = NormalBackground;
                recDisabled.Fill = DisabledBackground;
                recSelected.Fill = SelectedBackground;
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
            //return;
            //Point pt = e.GetCurrentPoint(recCP).RawPosition;

            //TiltEffect(pt);
        }

        private void mainGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsDisabled) return;

            _IsPressed = true;

            Point pt = e.GetCurrentPoint(recCP).RawPosition;
            recBackground.Fill = SelectedBackground;
            TiltEffect(pt);

            //e.Handled = PointerReleasedHandled;

            dtResetPressedState.Start(); //give it x seconds before we force an unpress
        }

        private void mainGrid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _doRelease(e);
        }


        void TiltTile_Click(object sender, RoutedEventArgs e)
        {
            _doRelease(e);
        }

        private void _doRelease(RoutedEventArgs e)
        {
            if (IsDisabled) return;
            if (!_IsPressed) return;

            kfX.Value = 0;
            kfY.Value = 0;
            kfScaleX.Value = 1.0;
            kfScaleY.Value = 1.0;
            recBackground.Fill = NormalBackground;
            sbTilt.Begin();

            if (ClickAction == eClickAction.Explode) Explode();

            if (Clicked != null && e is PointerRoutedEventArgs) Clicked(this, (PointerRoutedEventArgs)e);
            else if (Clicked != null) Clicked(this, null);
            
            if (ClickAction == eClickAction.Messenger && ClickMessengerIdentifier != null)
            {
                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("")
                {
                    Identifier = ClickMessengerIdentifier,
                    Action = ClickMessengerAction,
                    AggregateId = ClickMessengerAggregateId
                });
            }

            if (e != null && e is PointerRoutedEventArgs) ((PointerRoutedEventArgs)e).Handled = PointerReleasedHandled;
        }


        private void mainGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsPressed) return;
            kfX.Value = 0;
            kfY.Value = 0;
            kfScaleX.Value = 1.0;
            kfScaleY.Value = 1.0;
            recBackground.Fill = NormalBackground;

            if (_IsPressed) return;
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

        public void PrepareForShow()
        {
            mainGrid.Opacity = 0;
            ((CompositeTransform)mainGrid.RenderTransform).ScaleX = 0;
            ((CompositeTransform)mainGrid.RenderTransform).ScaleY = 0;
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
            if (imgBackground == null) return;
            imgBackground.Opacity = 0.1;
            imgBackground.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(uri);
            //imgBackground.Loaded += (o, e) => { sbShowImage.Begin(); };
            sbShowImage.Begin();
        }

        public void LoadPathIcon(string pathData, Brush iconColor = null, int width = 25, int height = 25)
        {
            if (ccIcon == null) return;
            if (iconColor == null) iconColor = new SolidColorBrush(Windows.UI.Colors.DimGray);
            if (width == 0) width = 25;
            if (height == 0) height = 25;

            string pthString = @"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                Data=""" + pathData + @""" />";
            Windows.UI.Xaml.Shapes.Path pth = (Windows.UI.Xaml.Shapes.Path)Windows.UI.Xaml.Markup.XamlReader.Load(pthString);
            pth.Stretch = Stretch.Uniform;
            pth.Fill = iconColor;
            pth.Width = width;
            pth.Height = height;

            if (pthMoreItems != null) pthMoreItems.Fill = iconColor;

            ccIcon.Content = pth;
        }

        public void Unload()
        {
            if (mainGrid == null) return;

            mainGrid.PointerMoved -= mainGrid_PointerMoved;
            mainGrid.PointerPressed -= mainGrid_PointerPressed;
            mainGrid.PointerReleased -= mainGrid_PointerReleased;
            mainGrid.PointerExited -= mainGrid_PointerExited;
            this.Click -= TiltTile_Click;
        }

     
    }
}
