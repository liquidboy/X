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

namespace X.UI.LiteDataGrid
{
    public sealed class LiteDataGrid : Control
    {

        ItemsControl _icMain;

        public LiteDataGrid()
        {
            this.DefaultStyleKey = typeof(LiteDataGrid);
            
            Loaded += LiteDataGrid_Loaded;
            Unloaded += LiteDataGrid_Unloaded;
        }

        private void LiteDataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            if(_icMain != null) _icMain.ItemsSource = null;
            ItemTemplate1 = null;
            ItemTemplate2 = null;

        }

        private void LiteDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected override void OnApplyTemplate()
        {
            //if control begins its life as invisible (visibility = collapsed) then OnApplyTemplate is not called BUT becareful because dependency properties OnPropertyChangedEvents do
            if (_icMain == null) _icMain = GetTemplateChild("icMain") as ItemsControl;


            base.OnApplyTemplate();
        }


        public DataTemplate ItemTemplate1
        {
            get { return (DataTemplate)GetValue(ItemTemplate1Property); }
            set { SetValue(ItemTemplate1Property, value); }
        }
        
        public DataTemplate ItemTemplate2
        {
            get { return (DataTemplate)GetValue(ItemTemplate2Property); }
            set { SetValue(ItemTemplate2Property, value); }
        }
        
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        
        public int ItemTemplateToUse
        {
            get { return (int)GetValue(ItemTemplateToUseProperty); }
            set { SetValue(ItemTemplateToUseProperty, value); }
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









        public static readonly DependencyProperty ItemTemplateToUseProperty =
            DependencyProperty.Register("ItemTemplateToUse", typeof(int), typeof(LiteDataGrid), new PropertyMetadata(0, OnPropertyItemTemplateToUseChanged));
        
        public static readonly DependencyProperty ItemTemplate1Property = DependencyProperty.Register("ItemTemplate1", typeof(DataTemplate), typeof(LiteDataGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemTemplate2Property = DependencyProperty.Register("ItemTemplate2", typeof(DataTemplate), typeof(LiteDataGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(LiteDataGrid), new PropertyMetadata(null));
        
        public static readonly DependencyProperty GlowAmountProperty = DependencyProperty.Register("GlowAmount", typeof(double), typeof(LiteDataGrid), new PropertyMetadata(3));

        public static readonly DependencyProperty GlowColorProperty = DependencyProperty.Register("GlowColor", typeof(Brush), typeof(LiteDataGrid), new PropertyMetadata(Colors.Black));

        public static readonly DependencyProperty FocusForegroundColorProperty = DependencyProperty.Register("FocusForegroundColor", typeof(Brush), typeof(LiteDataGrid), new PropertyMetadata(Colors.White));

        public static readonly DependencyProperty FocusHoverColorProperty = DependencyProperty.Register("FocusHoverColor", typeof(Brush), typeof(LiteDataGrid), new PropertyMetadata(Colors.DarkGray));

        public static readonly DependencyProperty FocusColorProperty = DependencyProperty.Register("FocusColor", typeof(Brush), typeof(LiteDataGrid), new PropertyMetadata(Colors.DarkGray));







        private static void OnPropertyItemTemplateToUseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            
            var instance = d as LiteDataGrid;
            if (instance._icMain == null) return;

            var index = (int)e.NewValue;
            if (index == 1) instance._icMain.ItemTemplate = instance.ItemTemplate1;
            else if (index == 2) instance._icMain.ItemTemplate = instance.ItemTemplate2;
            else instance._icMain.ItemTemplate = null;


        }
        
    }
}
