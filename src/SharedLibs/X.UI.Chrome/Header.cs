using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace X.UI.Chrome
{
    public sealed class Header : Control
    {
        EffectLayer.EffectLayer _bkgLayer;//x
        TextBlock _tbTitle;
        int RenderTargetIndexFor_tbTitle = 0;
        Grid _root;

        double bkgOffsetX = 0;
        double bkgOffsetY = 0;


        public Header()
        {
            this.DefaultStyleKey = typeof(Header);

            this.Loaded += Header_Loaded;
            this.Unloaded += Header_Unloaded;
        }

        private void Header_Unloaded(object sender, RoutedEventArgs e)
        {
           
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

            if (_tbTitle == null)
            {
                _tbTitle = GetTemplateChild("tbTitle") as TextBlock;
                _tbTitle.DataContext = this;
            }

            if (_bkgLayer != null && _tbTitle != null && _tbTitle.ActualWidth != 0) _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY);


            base.OnApplyTemplate();
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

            //Window.Current.SetTitleBar(recSmallTitle);
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

    }
}
