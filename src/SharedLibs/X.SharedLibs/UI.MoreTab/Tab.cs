using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace X.UI.MoreTab
{
    public sealed class Tab : Control
    {
        RadioButton _checkDragGrouping;
        RadioButton _checkDragReordering;

        public Tab()
        {
            this.DefaultStyleKey = typeof(Tab);

            this.Loaded += Tab_Loaded;
            this.Unloaded += Tab_Unloaded;
        }

        private void Tab_Unloaded(object sender, RoutedEventArgs e)
        {
            _checkDragGrouping.Checked -= _checkDragGrouping_Checked;
            _checkDragReordering.Checked -= _checkDragReordering_Checked;
        }

        private void Tab_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnApplyTemplate()
        {
            if (_checkDragGrouping == null) {
                _checkDragGrouping = GetTemplateChild("checkDragGrouping") as RadioButton;
                _checkDragGrouping.Checked += _checkDragGrouping_Checked;
            }
            if (_checkDragReordering == null)
            {
                _checkDragReordering = GetTemplateChild("checkDragReordering") as RadioButton;
                _checkDragReordering.Checked += _checkDragReordering_Checked;
            }



            base.OnApplyTemplate();
        }

        private void _checkDragReordering_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void _checkDragGrouping_Checked(object sender, RoutedEventArgs e)
        {



        }







        public Brush Accent1
        {
            get { return (Brush)GetValue(Accent1Property); }
            set { SetValue(Accent1Property, value); }
        }

        public Brush Accent2
        {
            get { return (Brush)GetValue(Accent2Property); }
            set { SetValue(Accent2Property, value); }
        }
        
        public int TabCount
        {
            get { return (int)GetValue(TabCountProperty); }
            set { SetValue(TabCountProperty, value); }
        }
        



        public static readonly DependencyProperty TabCountProperty =
            DependencyProperty.Register("TabCount", typeof(int), typeof(Tab), new PropertyMetadata(0));
        
        public static readonly DependencyProperty Accent2Property =
            DependencyProperty.Register("Accent2", typeof(Brush), typeof(Tab), new PropertyMetadata(null));
        
        public static readonly DependencyProperty Accent1Property =
            DependencyProperty.Register("Accent1", typeof(Brush), typeof(Tab), new PropertyMetadata(null));






    }
}
