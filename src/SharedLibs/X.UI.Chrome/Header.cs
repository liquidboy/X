using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using X.Win32;


namespace X.UI.Chrome
{
    public sealed class Header : Control
    {
        EffectLayer.EffectLayer _bkgLayer;//x
        TextBlock _tbTitle;
        int RenderTargetIndexFor_tbTitle = 0;
        Grid _root;
        Windows.UI.Xaml.Shapes.Rectangle _recSmallTitle;
        Button _tlMain;

        double bkgOffsetX = 0;
        double bkgOffsetY = 0;

        
        DispatcherTimer _dtChrome;
        NativeLib.Win32Point _curPos = new NativeLib.Win32Point();




        public Header()
        {
            this.DefaultStyleKey = typeof(Header);

            this.Loaded += Header_Loaded;
            this.Unloaded += Header_Unloaded;
        }

        private void Header_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_tlMain != null) _tlMain.Click -= _tlMain_Click;
        }

        private void Header_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bkgLayer != null)
            {
                var gt = _tbTitle.TransformToVisual(_bkgLayer);
                var pt = gt.TransformPoint(new Windows.Foundation.Point(0, 0));
                _bkgLayer.DrawUIElements(_tbTitle, offsetX: pt.X, offsetY: pt.Y);
                _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY, EffectLayer.EffectGraphType.Glow);
            }
        }

        protected override void OnApplyTemplate()
        {
            if(_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;

            if (_root == null) _root = GetTemplateChild("root") as Grid;
            if (_tlMain == null) { _tlMain = GetTemplateChild("tlMain") as Button; _tlMain.DataContext = this; _tlMain.Click += _tlMain_Click; }
            if (_recSmallTitle == null)
            {
                _recSmallTitle = GetTemplateChild("recSmallTitle") as Windows.UI.Xaml.Shapes.Rectangle;
                Window.Current.SetTitleBar(_recSmallTitle);

                _dtChrome = new DispatcherTimer();
                _dtChrome.Interval = new TimeSpan(0, 0, 0, 0, 15);
                _dtChrome.Tick += (object sender, object e) =>
                {
                    NativeLib.GetCursorPos(ref _curPos);
                    ChromeUpdate(_curPos);
                };
                if (EnableResizeFix) _dtChrome.Start();
                else _dtChrome.Stop();
            }

            if (_tbTitle == null)
            {
                _tbTitle = GetTemplateChild("tbTitle") as TextBlock;
                _tbTitle.DataContext = this;
            }

            if (_bkgLayer != null && _tbTitle != null && _tbTitle.ActualWidth != 0) _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY);


            base.OnApplyTemplate();
        }

        private void _tlMain_Click(object sender, RoutedEventArgs e)
        {
            EnableResizeFix = !EnableResizeFix;

            if (EnableResizeFix) _dtChrome.Start();
            else _dtChrome.Stop();
        }

        public void Invalidate(double offsetX = 0, double offsetY = 0) {
            if (_bkgLayer != null) { 
                var gt = _tbTitle.TransformToVisual(_bkgLayer);
                var pt = gt.TransformPoint(new Windows.Foundation.Point(0, 0));
                _bkgLayer.DrawUIElements(_tbTitle, RenderTargetIndexFor_tbTitle, offsetX + pt.X, offsetY + pt.Y);
            }
        }


        public void InitChrome(Application app, ApplicationView applicationView)
        {
            app.DebugSettings.IsTextPerformanceVisualizationEnabled = false;
            app.DebugSettings.IsBindingTracingEnabled = false;
            app.DebugSettings.IsOverdrawHeatMapEnabled = false;
            app.DebugSettings.EnableFrameRateCounter = false;


            var titleBar = applicationView.TitleBar;

            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ForegroundColor = Colors.Black;

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

        }





        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        
        public Color GlowColor
        {
            get { return (Color)GetValue(GlowColorProperty); }
            set { SetValue(GlowColorProperty, value); }
        }

        public double GlowAmount
        {
            get { return (double)GetValue(GlowAmountProperty); }
            set { SetValue(GlowAmountProperty, value); }
        }
        
        public ImageSource IconUri
        {
            get { return (ImageSource)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        public bool EnableResizeFix
        {
            get { return (bool)GetValue(EnableResizeFixProperty); }
            set { SetValue(EnableResizeFixProperty, value); }
        }









        public static readonly DependencyProperty EnableResizeFixProperty = DependencyProperty.Register("EnableResizeFix", typeof(bool), typeof(Header), new PropertyMetadata(false, OnEnableResizeFix));
        
        public static readonly DependencyProperty IconUriProperty = DependencyProperty.Register("IconUri", typeof(ImageSource), typeof(Header), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Header), new PropertyMetadata(string.Empty, OnPropertyChanged));

        public static readonly DependencyProperty GlowAmountProperty = DependencyProperty.Register("GlowAmount", typeof(double), typeof(Header), new PropertyMetadata(2, OnPropertyChanged));

        public static readonly DependencyProperty GlowColorProperty = DependencyProperty.Register("GlowColor", typeof(Color), typeof(Header), new PropertyMetadata(Colors.Black, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as Header;
            if (d == null)
                return;

            if (instance._root != null)
            {
                instance.Invalidate();
            }
        }

        private static void OnEnableResizeFix(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as Header;
            if (d == null)
                return;


            instance._dtChrome?.Start();
        }


        private void ChromeUpdate(NativeLib.Win32Point pt)
        {
            var xRightMost = Window.Current.Bounds.Left + Window.Current.Bounds.Width - 200;

            if (pt.x > xRightMost) return;

            if (pt.y < Window.Current.Bounds.Top + 4)
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeNorthSouth, 66651);
            else if (pt.y < Window.Current.Bounds.Top + 15)
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.SizeAll, 66652);
            else if (pt.y < Window.Current.Bounds.Top + 45)
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 66653);
        }


    }
}
