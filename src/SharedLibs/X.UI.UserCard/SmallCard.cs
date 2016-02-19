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
    public sealed class SmallCard : Control
    {
        EffectLayer.EffectLayer _bkgLayer;//x

        StackPanel _root;
        int RenderTargetIndexFor_Root = 0;

        double bkgOffsetX = 0;
        double bkgOffsetY = 0;

        public SmallCard()
        {
            this.DefaultStyleKey = typeof(SmallCard);

            Loaded += SmallCard_Loaded;
            Unloaded += SmallCard_Unloaded;
        }

        private void SmallCard_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void SmallCard_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bkgLayer != null)
            {
                _bkgLayer.DrawUIElements(_root); 
                _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY);
            }
        }

        protected override void OnApplyTemplate()
        {
            if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;

            if (_root == null)
            {
                _root = GetTemplateChild("root") as StackPanel;
                _root.DataContext = this;
            }

            if (_bkgLayer != null && _root != null && _root.ActualWidth != 0) _bkgLayer.InitLayer(_root.ActualWidth, _root.ActualHeight, bkgOffsetX, bkgOffsetY);

            base.OnApplyTemplate();
        }
        
        public void Invalidate(double offsetX = 0, double offsetY = 0) { _bkgLayer?.DrawUIElements(_root, RenderTargetIndexFor_Root, offsetX, offsetY); }






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











        public static readonly DependencyProperty IsAvatarOnRightProperty = DependencyProperty.Register("IsAvatarOnRight", typeof(bool), typeof(SmallCard), new PropertyMetadata(true, OnPropertyChanged));

        public static readonly DependencyProperty AvatarUriProperty = DependencyProperty.Register("AvatarUri", typeof(ImageSource), typeof(SmallCard), new PropertyMetadata(null));
        
        public static readonly DependencyProperty GlowAmountProperty = DependencyProperty.Register("GlowAmount", typeof(double), typeof(SmallCard), new PropertyMetadata(2, OnPropertyChanged));

        public static readonly DependencyProperty GlowColorProperty = DependencyProperty.Register("GlowColor", typeof(Color), typeof(SmallCard), new PropertyMetadata(Colors.Black, OnPropertyChanged));
        
        public static readonly DependencyProperty SecondaryNameProperty = DependencyProperty.Register("SecondaryName", typeof(string), typeof(SmallCard), new PropertyMetadata(string.Empty));
        
        public static readonly DependencyProperty PrimaryNameProperty = DependencyProperty.Register("PrimaryName", typeof(string), typeof(SmallCard), new PropertyMetadata(string.Empty));
        
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as SmallCard;
            if (d == null)
                return;

            if (instance._root != null)
            {
                instance.Invalidate();
            }
        }

      

    }
}
