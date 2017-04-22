using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
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

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed partial class InputBoxView : UserControl
    {
        //public event EventHandler OnClosing;
        //private DispatcherTimer dtClose;

        private Brush _normalIconColor = new SolidColorBrush(Colors.Purple);
        private GeneralSystemWideMessage _msgToPassAlong;

        public Brush BackgroundFill
        {
            get { return (Brush)GetValue(BackgroundFillProperty); }
            set { SetValue(BackgroundFillProperty, value); }
        }

        
        public static readonly DependencyProperty BackgroundFillProperty =
            DependencyProperty.Register("BackgroundFill", typeof(Brush), typeof(InputBoxView), new PropertyMetadata(0, BackgroundFillPropertyChanged));

        private static void BackgroundFillPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            InputBoxView ctl = (InputBoxView)obj;

            ctl.layoutRoot.Background = (Brush)args.NewValue;

        }



        public Brush MessageTextForegroundColor
        {
            get { return (Brush)GetValue(MessageTextForegroundColorProperty); }
            set { SetValue(MessageTextForegroundColorProperty, value); }
        }


        public static readonly DependencyProperty MessageTextForegroundColorProperty =
            DependencyProperty.Register("MessageTextForegroundColor", typeof(Brush), typeof(InputBoxView), new PropertyMetadata(0, MessageTextForegroundColorPropertyChanged));

        private static void MessageTextForegroundColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            InputBoxView ctl = (InputBoxView)obj;

            ctl.lblMessage.Foreground = (Brush)args.NewValue;
        }

        public Brush CountdownBackgroundColor
        {
            get { return (Brush)GetValue(CountdownBackgroundColorProperty); }
            set { SetValue(CountdownBackgroundColorProperty, value); }
        }


        public static readonly DependencyProperty CountdownBackgroundColorProperty =
            DependencyProperty.Register("CountdownBackgroundColor", typeof(Brush), typeof(InputBoxView), new PropertyMetadata(0, CountdownBackgroundColorPropertyChanged));

        private static void CountdownBackgroundColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            InputBoxView ctl = (InputBoxView)obj;

            ctl.recTimeLeft.Fill = (Brush)args.NewValue;
            ctl.recTimeLeft.Stroke = (Brush)args.NewValue;

        }

        

        public void  Show()
        {
            sbShow.Begin();
            //sbCountdown.Begin();

        }


        public void Hide()
        {
            sbHide.Begin();
            sbHide.Completed += (o, args) => { 
                //if (OnClosing != null) OnClosing(this, EventArgs.Empty);
                //sbCountdown.Stop();
                sbHide.Stop();
                sbShow.Stop();
                //dtClose = null; 
            };
        }


        public string YesMessengerContent { get; set; }
        public string YesMessengerIdentifier { get; set; }
        public string NoMessengerContent { get; set; }
        public string NoMessengerIdentifier { get; set; }

        public InputBoxView(
            string msg, 
            string title,  
            Brush brushIcon, 
            string metroIcon = "", 
            string imageIcon = "", 
            double scaleIcon = 1.0,
            double translateXIcon = 0,
            double translateYIcon = 0,
            string yesLabel = "Yes",
            string yesMessengerContent = "",
            string yesMessengerIdentifier = "",
            string noLabel = "No",
            string noMessengerContent = "",
            string noMessengerIdentifier = "",
            GeneralSystemWideMessage msgToPassAlong = null
            )
        {
            this.InitializeComponent();

            lblTitle.Text = title;
            lblMessage.Text = msg;
            _msgToPassAlong = msgToPassAlong;

            //((DoubleAnimation)sbCountdown.Children[0]).Duration = new Duration(TimeSpan.FromSeconds(timeToLive));

            
            if(imageIcon!=string.Empty)
                LoadImage(imageIcon);
            else
                LoadMetroIcon(
                metroIcon,
                iconColor: brushIcon,
                scaleIcon: scaleIcon,
                translateXIcon: translateXIcon,
                translateYIcon: translateYIcon);


            butYes.Content = yesLabel;
            YesMessengerContent = yesMessengerContent;
            YesMessengerIdentifier = yesMessengerIdentifier;

            butNo.Content = noLabel;
            NoMessengerContent = noMessengerContent;
            NoMessengerIdentifier = noMessengerIdentifier;



            //dtClose = new DispatcherTimer();
            //dtClose.Interval = TimeSpan.FromSeconds(timeToLive);
            //dtClose.Tick += (o, e) => { 
            //    //sbCountdown.Stop(); 
            //    dtClose.Stop(); 
            //    this.Hide(); };
            //dtClose.Start();
        }

        private void LoadImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            layoutRoot.ColumnDefinitions[0].Width = new GridLength(250);

            grdIconImage.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            grdIconImage.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;

            Image img = new Image();
            img.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            img.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            img.Stretch = Stretch.Uniform;
            //img.Stretch = Stretch.None;
            img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(imageUrl));

            grdIconImage.Children.Add(img);

        }

        public void LoadMetroIcon(
            string key, 
            Brush iconColor = null, 
            double rotation = 0, 
            double scaleIcon = 1, 
            double translateXIcon = 0,
            double translateYIcon = 0)
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
            ((CompositeTransform)grdIconImage.RenderTransform).TranslateX = translateXIcon;
            ((CompositeTransform)grdIconImage.RenderTransform).TranslateY = translateYIcon;
        }

        private void butYes_Click(object sender, RoutedEventArgs e)
        {
            GeneralSystemWideMessage msgToSend;

            if (_msgToPassAlong != null)
            {
                msgToSend = _msgToPassAlong.Clone();
                msgToSend.Identifier = YesMessengerIdentifier;
                msgToSend.Action = YesMessengerContent;
                msgToSend.Text1 = tbSaySomething.Text;
                //msgToSend.Content = YesMessengerContent;
            }
            else msgToSend = new GeneralSystemWideMessage(YesMessengerContent) { Identifier = YesMessengerIdentifier, Action = YesMessengerContent , Text1 = tbSaySomething.Text};

            Messenger.Default.Send<GeneralSystemWideMessage>(msgToSend);
        }

        private void butNo_Click(object sender, RoutedEventArgs e)
        {
            GeneralSystemWideMessage msgToSend;

            if (_msgToPassAlong != null)
            {
                msgToSend = _msgToPassAlong.Clone();
                msgToSend.Identifier = NoMessengerIdentifier;
                msgToSend.Action = NoMessengerContent;
                //msgToSend.Content = NoMessengerContent;
            }
            else msgToSend = new GeneralSystemWideMessage(NoMessengerContent) { Identifier = NoMessengerIdentifier, Action = NoMessengerContent };

            Messenger.Default.Send<GeneralSystemWideMessage>(msgToSend);

        }
    }
}
