using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace X.UI.RichButton
{


    public sealed class RichButton : Control
    {
        EffectLayer.EffectLayer _bkgLayer;//x
        TextBlock _tbContent;
        Button _butRoot;
        X.UI.Path.Path _xuiIco;
        Grid _grdTooltip;
        Grid _grdTTContainer;
        Windows.UI.Xaml.Shapes.Rectangle _rect;


        public event Windows.UI.Xaml.RoutedEventHandler Click;


        
        public RichButton()
        {
            this.DefaultStyleKey = typeof(RichButton);

            Loaded += RichButton_Loaded;
            Unloaded += RichButton_Unloaded;
        }


        
        private void RichButton_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_butRoot != null) {
                _butRoot.Click -= _butRoot_Click;
                _butRoot.PointerEntered -= _butRoot_PointerEntered;
                _butRoot.PointerExited -= _butRoot_PointerExited;
            }
        }


        
        private void RichButton_Loaded(object sender, RoutedEventArgs e)
        {
            _bkgLayer.InitLayer(_butRoot.ActualWidth, _butRoot.ActualHeight);
        }



        protected override void OnApplyTemplate()
        {
            if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;
            
            if (_butRoot == null)
            {
                _butRoot = GetTemplateChild("butRoot") as Button;
                _butRoot.Click += _butRoot_Click;
                _butRoot.PointerEntered += _butRoot_PointerEntered;
                _butRoot.PointerExited += _butRoot_PointerExited;
            }

            if (_tbContent == null) _tbContent = GetTemplateChild("tbContent") as TextBlock;
            
            if (_xuiIco == null) _xuiIco = GetTemplateChild("xuiIco") as X.UI.Path.Path;
            
            if (_grdTooltip == null) _grdTooltip = GetTemplateChild("grdTooltip") as Grid;
            
            if (_grdTTContainer == null) _grdTTContainer = GetTemplateChild("grdTTContainer") as Grid;
            
            _rect = GetTemplateChild("rect") as Windows.UI.Xaml.Shapes.Rectangle;


            base.OnApplyTemplate();
        }










        public double IcoSize
        {
            get { return (double)GetValue(IcoSizeProperty); }
            set { SetValue(IcoSizeProperty, value); }
        }

        public Brush TooltipBackground
        {
            get { return (Brush)GetValue(TooltipBackgroundProperty); }
            set { SetValue(TooltipBackgroundProperty, value); }
        }

        public bool ShowGlowArea
        {
            get { return (bool)GetValue(ShowGlowAreaProperty); }
            set { SetValue(ShowGlowAreaProperty, value); }
        }

        public double GlowAmount
        {
            get { return (double)GetValue(GlowAmountProperty); }
            set { SetValue(GlowAmountProperty, value); }
        }

        public Windows.UI.Color GlowColor
        {
            get { return (Windows.UI.Color)GetValue(GlowColorProperty); }
            set { SetValue(GlowColorProperty, value); }
        }

        public string Tooltip
        {
            get { return (string)GetValue(TooltipProperty); }
            set { SetValue(TooltipProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public X.UI.Path.PathType PathType
        {
            get { return (X.UI.Path.PathType)GetValue(PathTypeProperty); }
            set { SetValue(PathTypeProperty, value); }
        }





        public static readonly DependencyProperty GlowAmountProperty = DependencyProperty.Register("GlowAmount", typeof(double), typeof(RichButton), new PropertyMetadata(0));

        public static readonly DependencyProperty ShowGlowAreaProperty = DependencyProperty.Register("ShowGlowArea", typeof(bool), typeof(RichButton), new PropertyMetadata(false));

        public static readonly DependencyProperty TooltipBackgroundProperty =
     DependencyProperty.Register("TooltipBackground", typeof(Brush), typeof(RichButton), new PropertyMetadata(new SolidColorBrush(Windows.UI.Colors.Black)));
        
        public static readonly DependencyProperty GlowColorProperty =
            DependencyProperty.Register("GlowColor", typeof(Windows.UI.Color), typeof(RichButton), new PropertyMetadata(Windows.UI.Colors.Black));

        public static readonly DependencyProperty IcoSizeProperty = DependencyProperty.Register("IcoSize", typeof(double), typeof(RichButton), new PropertyMetadata(20));

        public static readonly DependencyProperty TooltipProperty = DependencyProperty.Register("Tooltip", typeof(string), typeof(RichButton), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(RichButton), new PropertyMetadata(""));
        
        public static readonly DependencyProperty PathTypeProperty = DependencyProperty.Register("PathType", typeof( X.UI.Path.PathType), typeof(RichButton), new PropertyMetadata(0));

















        private void _butRoot_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
            _grdTooltip.Visibility = Visibility.Collapsed;
        }

        private void _butRoot_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
            _grdTooltip.Visibility = Visibility.Visible;
        }

        private void _butRoot_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null) Click(sender, e);
        }



    }
}


//  https://social.msdn.microsoft.com/Forums/en-US/8a8049b5-16a8-45ea-8ce1-04cd6f9b21d3/how-to-create-a-custom-routed-event-in-metro-app?forum=winappswithcsharp
