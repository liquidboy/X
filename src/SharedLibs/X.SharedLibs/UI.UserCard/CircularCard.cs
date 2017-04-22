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


namespace X.UI.UserCard
{
    public sealed class CircularCard : Control
    {
        //EffectLayer.EffectLayer _bkgLayer;//x

        StackPanel _root;
        int RenderTargetIndexFor_Root = 0;
        //DispatcherTimer dtInvalidate;

        double bkgOffsetX = 0;
        double bkgOffsetY = 0;

        public CircularCard()
        {
            this.DefaultStyleKey = typeof(CircularCard);

            //dtInvalidate = new DispatcherTimer();
            //dtInvalidate.Tick += DtInvalidate_Tick;
            //dtInvalidate.Interval = TimeSpan.FromMilliseconds(100);

            Loaded += CircularCard_Loaded;
            Unloaded += CircularCard_Unloaded;

        }

        //private void DtInvalidate_Tick(object sender, object e)
        //{
        //    //dtInvalidate.Stop();
        //    Invalidate();
        //}

        private void CircularCard_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void CircularCard_Loaded(object sender, RoutedEventArgs e)
        {
            //if (_bkgLayer != null)
            //{
            //    _bkgLayer.DrawUIElements(_root, offsetX: 3);
            //    _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY, EffectLayer.EffectGraphType.Glow);
            //    //dtInvalidate.Start();
            //}
        }

        protected override void OnApplyTemplate()
        {
            //if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;

            if (_root == null)
            {
                _root = GetTemplateChild("root") as StackPanel;
                _root.DataContext = this;
            }

            //if (_bkgLayer != null && _root != null && _root.ActualWidth != 0) _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY, EffectLayer.EffectGraphType.Glow);

            base.OnApplyTemplate();
        }
        
        public void Invalidate(double offsetX = 3, double offsetY = 0) {
            //_bkgLayer?.DrawUIElements(_root, RenderTargetIndexFor_Root, offsetX, offsetY);
        }






        public string PrimaryName
        {
            get { return (string)GetValue(PrimaryNameProperty); }
            set { SetValue(PrimaryNameProperty, value); }
        }
        
        public string SecondaryName
        {
            get { return (string)GetValue(SecondaryNameProperty); }
            set { SetValue(SecondaryNameProperty, value); }
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
        
        public ImageSource AvatarUri
        {
            get { return (ImageSource)GetValue(AvatarUriProperty); }
            set { SetValue(AvatarUriProperty, value); }
        }



        public bool IsAvatarOnRight
        {
            get { return (bool)GetValue(IsAvatarOnRightProperty); }
            set { SetValue(IsAvatarOnRightProperty, value); }
        }
        
        public double BlurOffsetX
        {
            get { return (double)GetValue(BlurOffsetXProperty); }
            set { SetValue(BlurOffsetXProperty, value); }
        }
        
        public double BlurOffsetY
        {
            get { return (double)GetValue(BlurOffsetYProperty); }
            set { SetValue(BlurOffsetYProperty, value); }
        }
        
        public HorizontalAlignment AlignImage
        {
            get { return (HorizontalAlignment)GetValue(AlignImageProperty); }
            set { SetValue(AlignImageProperty, value); }
        }









        public static readonly DependencyProperty AlignImageProperty = DependencyProperty.Register("AlignImage", typeof(HorizontalAlignment), typeof(CircularCard), new PropertyMetadata(HorizontalAlignment.Center));

        public static readonly DependencyProperty BlurOffsetYProperty = DependencyProperty.Register("BlurOffsetY", typeof(double), typeof(CircularCard), new PropertyMetadata(0));

        public static readonly DependencyProperty BlurOffsetXProperty = DependencyProperty.Register("BlurOffsetX", typeof(double), typeof(CircularCard), new PropertyMetadata(0));
        
        public static readonly DependencyProperty IsAvatarOnRightProperty = DependencyProperty.Register("IsAvatarOnRight", typeof(bool), typeof(CircularCard), new PropertyMetadata(false, OnPropertyChanged));

        public static readonly DependencyProperty AvatarUriProperty = DependencyProperty.Register("AvatarUri", typeof(ImageSource), typeof(CircularCard), new PropertyMetadata(null, OnPropertyChanged));
        
        public static readonly DependencyProperty GlowAmountProperty = DependencyProperty.Register("GlowAmount", typeof(double), typeof(CircularCard), new PropertyMetadata(2, OnPropertyChanged));

        public static readonly DependencyProperty GlowColorProperty = DependencyProperty.Register("GlowColor", typeof(Color), typeof(CircularCard), new PropertyMetadata(Colors.Black, OnPropertyChanged));
        
        public static readonly DependencyProperty SecondaryNameProperty = DependencyProperty.Register("SecondaryName", typeof(string), typeof(CircularCard), new PropertyMetadata(string.Empty));
        
        public static readonly DependencyProperty PrimaryNameProperty = DependencyProperty.Register("PrimaryName", typeof(string), typeof(CircularCard), new PropertyMetadata(string.Empty));
        
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CircularCard;
            if (d == null)
                return;

            if (instance._root != null)
            {
                instance.Invalidate();
            }
        }

        
    }
}
