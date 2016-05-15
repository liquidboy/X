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
using Windows.UI.Xaml.Media.Imaging;

namespace X.UI.Toolbar
{
    public sealed class Toolbar : Control
    {

        StackPanel _spExtensions;
        public event Windows.UI.Xaml.RoutedEventHandler Click;



        public Toolbar()
        {
            this.DefaultStyleKey = typeof(Toolbar);

            this.Loaded += Toolbar_Loaded;
            this.Unloaded += Toolbar_Unloaded;
        }




        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public bool HasExtensions
        {
            get { return (bool)GetValue(HasExtensionsProperty); }
            set { SetValue(HasExtensionsProperty, value); }
        }




        public static readonly DependencyProperty HasExtensionsProperty = DependencyProperty.Register("HasExtensions", typeof(bool), typeof(Toolbar), new PropertyMetadata(false));
        
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Toolbar), new PropertyMetadata(Orientation.Horizontal));

        

        private void Toolbar_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_spExtensions != null) {

                foreach (var item in _spExtensions.Children) {
                    if (item is ImageButton) {
                        ((ImageButton)item).Click -= NewIcon_Click;
                    }
                }
               
                _spExtensions.Children.Clear();
                _itemsToDraw.Clear();

            }
        }

        private void Toolbar_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        List<ImageButton> _itemsToDraw = new List<ImageButton>();
        public void ClearAll() {
            if (_spExtensions == null) return;
            if (_spExtensions.Children == null) return;
            foreach (var ext in _spExtensions.Children) {
                ((ImageButton)ext).Click -= NewIcon_Click;
            }
            _spExtensions.Children.Clear();
            HasExtensions = false;
        }
        public void AddItem(string iconUrl, double height, Guid uniqueId, BitmapImage iconBitmap = null) {
            ImageButton newIcon;

            if (Orientation == Orientation.Horizontal)
                newIcon = new ImageButton() { IconUri = iconUrl, Height = height, ExtensionUniqueId = uniqueId, Margin = new Thickness(10, 0, 0, 0), IconBitmap = iconBitmap };
            else
                newIcon = new ImageButton() { IconUri = iconUrl, Height = height, ExtensionUniqueId = uniqueId, Margin = new Thickness(0, 0, 0, 10), IconBitmap = iconBitmap };

            newIcon.Click += NewIcon_Click;

            if (_spExtensions == null) _itemsToDraw.Add(newIcon);
            else  _spExtensions.Children.Add(newIcon);

            HasExtensions = true; 
        }




        private void NewIcon_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null) Click(sender, e);
        }

        protected override void OnApplyTemplate()
        {
            if (_spExtensions == null) _spExtensions = GetTemplateChild("spExtensions") as StackPanel;

            foreach(var item in _itemsToDraw) _spExtensions.Children.Add(item);
            
            base.OnApplyTemplate();
        }
    }
}
