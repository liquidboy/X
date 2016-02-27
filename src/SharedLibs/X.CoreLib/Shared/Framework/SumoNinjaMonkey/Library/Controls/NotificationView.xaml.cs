using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed partial class NotificationView : UserControl
    {
        public event EventHandler OnClosing;
        private DispatcherTimer dtClose;

        private Brush _normalIconColor = new SolidColorBrush(Colors.Purple);




        public Brush MessageTextForegroundColor
        {
            get { return (Brush)GetValue(MessageTextForegroundColorProperty); }
            set { SetValue(MessageTextForegroundColorProperty, value); }
        }


        public static readonly DependencyProperty MessageTextForegroundColorProperty =
            DependencyProperty.Register("MessageTextForegroundColor", typeof(Brush), typeof(NotificationView), new PropertyMetadata(0, MessageTextForegroundColorPropertyChanged));

        private static void MessageTextForegroundColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NotificationView ctl = (NotificationView)obj;

            ctl.lblMessage.Foreground = (Brush)args.NewValue;

        }

        public Brush CountdownBackgroundColor
        {
            get { return (Brush)GetValue(CountdownBackgroundColorProperty); }
            set { SetValue(CountdownBackgroundColorProperty, value); }
        }


        public static readonly DependencyProperty CountdownBackgroundColorProperty =
            DependencyProperty.Register("CountdownBackgroundColor", typeof(Brush), typeof(NotificationView), new PropertyMetadata(0, CountdownBackgroundColorPropertyChanged));

        private static void CountdownBackgroundColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NotificationView ctl = (NotificationView)obj;

            ctl.recTimeLeft.Fill = (Brush)args.NewValue;
            ctl.recTimeLeft.Stroke = (Brush)args.NewValue;

        }

        

        public void Show()
        {
            sbShow.Begin();
            sbCountdown.Begin();
        }

        public void Hide()
        {
            sbHide.Begin();
            sbHide.Completed += (o, args) => { 
                if (OnClosing != null) OnClosing(this, EventArgs.Empty);
                sbCountdown.Stop();
                sbHide.Stop();
                sbShow.Stop();
                dtClose = null; };
        }

        public NotificationView(string msg, string title, bool autoHide, double timeToLive, string metroIcon = "", string imageIcon = "", double scaleIcon = 1.0)
        {
            this.InitializeComponent();

            lblTitle.Text = title;
            lblMessage.Text = msg;

            ((DoubleAnimation)sbCountdown.Children[0]).Duration = new Duration(TimeSpan.FromSeconds(timeToLive));

            if (imageIcon != string.Empty) LoadImage(imageIcon); 
            else  LoadMetroIcon(metroIcon, iconColor:null, scaleIcon: scaleIcon);

            dtClose = new DispatcherTimer();
            dtClose.Interval = TimeSpan.FromSeconds(timeToLive);
            dtClose.Tick += (o, e) => { 
                //sbCountdown.Stop(); 
                dtClose.Stop(); 
                this.Hide(); };
            dtClose.Start();
        }

        private void LoadImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            grdIconImage.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            grdIconImage.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            Image img = new Image();
            img.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            img.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            img.Stretch = Stretch.UniformToFill;
            img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(imageUrl));

            grdIconImage.Children.Add(img);

        }

        private void LoadMetroIcon(string key, Brush iconColor = null, double rotation = 0, double scaleIcon = 1)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (iconColor == null) iconColor = new SolidColorBrush(Windows.UI.Colors.White);
            _normalIconColor = iconColor;

            string temp = (string)Application.Current.Resources[key];
            string pthString = @"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                Data=""" + temp + @""" />";
            Windows.UI.Xaml.Shapes.Path pth = (Windows.UI.Xaml.Shapes.Path)Windows.UI.Xaml.Markup.XamlReader.Load(pthString);
            pth.Stretch = Stretch.Uniform;
            pth.Fill = iconColor;
            pth.Width = 25;
            pth.Height = 25;


            grdIconImage.Children.Add(pth);

            ((CompositeTransform)grdIconImage.RenderTransform).Rotation = rotation;
            ((CompositeTransform)grdIconImage.RenderTransform).ScaleX = scaleIcon;
            ((CompositeTransform)grdIconImage.RenderTransform).ScaleY = scaleIcon;
        }
    }
}
