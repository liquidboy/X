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
    public sealed class ImageButton : Control
    {
        Border rootLayout;
        Image imgMain;
        public event Windows.UI.Xaml.RoutedEventHandler Click;

        public ImageButton()
        {
            this.DefaultStyleKey = typeof(ImageButton);

            this.Loaded += ImageButton_Loaded;
            this.Unloaded += ImageButton_Unloaded;
        }

        private void ImageButton_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void ImageButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnApplyTemplate()
        {

            if (rootLayout == null)
            {
                rootLayout = (Border)GetTemplateChild("rootLayout");
                //rootLayoutScaleTransform = ((ScaleTransform)((TransformGroup)rootLayout.RenderTransform).Children[0]);
                rootLayout.PointerEntered += RootLayout_PointerEntered;
                rootLayout.PointerExited += RootLayout_PointerExited;
                rootLayout.PointerPressed += RootLayout_PointerPressed;
                rootLayout.PointerReleased += RootLayout_PointerReleased;

                imgMain = (Image)GetTemplateChild("imgMain");
                imgMain.Source = new BitmapImage(new Uri(IconUri));
            }

            base.OnApplyTemplate();
        }




        public Guid ExtensionUniqueId
        {
            get { return (Guid)GetValue(ExtensionUniqueIdProperty); }
            set { SetValue(ExtensionUniqueIdProperty, value); }
        }

        public string IconUri
        {
            get { return (string)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }



        public static readonly DependencyProperty IconUriProperty = DependencyProperty.Register("IconUri", typeof(string), typeof(ImageButton), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ExtensionUniqueIdProperty = DependencyProperty.Register("ExtensionUniqueId", typeof(Guid), typeof(ImageButton), new PropertyMetadata(Guid.Empty));



        private void RootLayout_PointerReleased(object sender, PointerRoutedEventArgs e)
        { 
            VisualStateManager.GoToState(this, "Normal", true);
            if (Click != null) Click(this, new ToolbarRoutedEventArgs() { UniqueGuid = ExtensionUniqueId } );
        }

        private void RootLayout_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Pressed", true);
        }

        private void RootLayout_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", true);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void RootLayout_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Hover", true);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
        }
    }

    



}
