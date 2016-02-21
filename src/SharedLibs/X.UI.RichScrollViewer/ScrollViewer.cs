using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace X.UI.RichScrollViewer
{
    public sealed class ScrollViewer : ContentControl
    {

        Grid _rootContainer;
        //ScrollViewer _root;  //wtf : due to casting weirdness below in onapplytemplate i had to make it a content control
        ContentControl _root;  //wtf : see above
        int RenderTargetIndexFor_root = 0;
        EffectLayer.EffectLayer _bkgLayer;

        double bkgOffsetX = 0;
        double bkgOffsetY = 0;

        public ScrollViewer()
        {
            this.DefaultStyleKey = typeof(ScrollViewer);

            this.Loaded += ScrollViewer_Loaded;
            this.Unloaded += ScrollViewer_Unloaded;
        }

        public void Invalidate(double offsetX = 0, double offsetY = 0) { _bkgLayer?.DrawUIElements(_root, RenderTargetIndexFor_root, offsetX, offsetY); }


        private void ScrollViewer_Unloaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bkgLayer != null)
            {
                var effectType = EffectLayer.EffectGraphType.Glow;
                
                _bkgLayer.DrawUIElements(_root);  //will draw at index 0 (RenderTargetIndexFor_icTabList)
                _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY, effectType);
            }
        }

        protected override void OnApplyTemplate()
        {

            if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;
            if (_rootContainer == null) _rootContainer = GetTemplateChild("rootContainer") as Grid;

            if (_root == null)
            {
                //wtf : as ScrollViewer fails 
                //_root = GetTemplateChild("root") as ScrollViewer;
                //_root = _rootContainer.FindName("root")  as ScrollViewer;   

                //hack : because of the weirdness above i had to make it a contentcontrol
                _root = GetTemplateChild("root") as ContentControl;
            }

            var effectType = EffectLayer.EffectGraphType.Glow;

            if (_bkgLayer != null && _root != null && _root.ActualWidth != 0) _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY, effectType);


            base.OnApplyTemplate();
        }









        public Brush FocusColor
        {
            get { return (Brush)GetValue(FocusColorProperty); }
            set { SetValue(FocusColorProperty, value); }
        }

        public Brush FocusHoverColor
        {
            get { return (Brush)GetValue(FocusHoverColorProperty); }
            set { SetValue(FocusHoverColorProperty, value); }
        }

        public Brush FocusForegroundColor
        {
            get { return (Brush)GetValue(FocusForegroundColorProperty); }
            set { SetValue(FocusForegroundColorProperty, value); }
        }


        public Brush GlowColor
        {
            get { return (Brush)GetValue(GlowColorProperty); }
            set { SetValue(GlowColorProperty, value); }
        }

        public double GlowAmount
        {
            get { return (double)GetValue(GlowAmountProperty); }
            set { SetValue(GlowAmountProperty, value); }
        }







        public static readonly DependencyProperty GlowAmountProperty = DependencyProperty.Register("GlowAmount", typeof(double), typeof(ScrollViewer), new PropertyMetadata(3));

        public static readonly DependencyProperty GlowColorProperty = DependencyProperty.Register("GlowColor", typeof(Brush), typeof(ScrollViewer), new PropertyMetadata(Colors.Black, OnPropertyChanged));

        public static readonly DependencyProperty FocusForegroundColorProperty = DependencyProperty.Register("FocusForegroundColor", typeof(Brush), typeof(ScrollViewer), new PropertyMetadata(Colors.White, OnPropertyChanged));

        public static readonly DependencyProperty FocusHoverColorProperty = DependencyProperty.Register("FocusHoverColor", typeof(Brush), typeof(ScrollViewer), new PropertyMetadata(Colors.DarkGray, OnPropertyChanged));

        public static readonly DependencyProperty FocusColorProperty = DependencyProperty.Register("FocusColor", typeof(Brush), typeof(ScrollViewer), new PropertyMetadata(Colors.DarkGray, OnPropertyChanged));




        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as ScrollViewer;
            if (d == null)
                return;

            if (instance._bkgLayer != null)
            {
                instance.Invalidate();
            }
        }
    }
}
