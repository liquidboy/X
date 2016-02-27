

using System;
using SumoNinjaMonkey.Framework.Controls;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
namespace SumoNinjaMonkey.Framework.Services
{
    public class NotificationService
    {
        private static Grid _rootControl;
        private static StackPanel _MsgboxContainer;
        public static void Init(Grid control, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Right)
        {
            NotificationService._rootControl = control;
            _MsgboxContainer = new StackPanel();
            _MsgboxContainer.Orientation = Orientation.Vertical;
            _MsgboxContainer.HorizontalAlignment = horizontalAlignment;
            _MsgboxContainer.VerticalAlignment = VerticalAlignment.Top;
            _MsgboxContainer.ChildrenTransitions = new Windows.UI.Xaml.Media.Animation.TransitionCollection();
            _MsgboxContainer.ChildrenTransitions.Add(new Windows.UI.Xaml.Media.Animation.RepositionThemeTransition());
            _rootControl.Children.Add(_MsgboxContainer);
        }


        public async static void Show(string message, string title, Brush foregroundTextBrush, Brush countdownBackgroundBrush, double timeToLive, bool autoHide = false, double width = 300, double height = 180,  string metroIcon = "", string imageIcon = "", double scaleIcon = 1)
        {

            if (NotificationService._rootControl != null && message != null)
            {
                DispatchedHandler invokedHandler = new DispatchedHandler(() =>
                {
                    if (NotificationService._rootControl == null) //|| MsgBoxService._rootControl.Visibility == Visibility.Visible)
                    {
                        return;
                    }
                    NotificationService._rootControl.Visibility = Visibility.Visible;
                    NotificationView view = new NotificationView(message, "", autoHide, timeToLive, metroIcon, imageIcon: imageIcon, scaleIcon: scaleIcon);
                    view.Width = width;
                    view.Height = height;
                    view.Margin = new Thickness(3);
                    //view.HorizontalAlignment = horizontalAlignment;
                    //view.VerticalAlignment = VerticalAlignment.Top;
                    
                    view.MessageTextForegroundColor = foregroundTextBrush;
                    view.CountdownBackgroundColor = countdownBackgroundBrush;
                    view.Show();
                    view.OnClosing += new EventHandler(NotificationService.view_OnClosing);

                    NotificationService._MsgboxContainer.Children.Insert(0, view);
                    
                });
                await NotificationService._rootControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);
            }
        }

        public async static void Show(string message, string title, bool autoHide = false, double width = 300, double height = 180,  double timeToLive = 5)
        {
            Show(message, title, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Yellow), timeToLive, autoHide, width, height);
        }


        private async static void view_OnClosing(object sender, EventArgs e)
        {
            DispatchedHandler invokedHandler = new DispatchedHandler(() => {
                NotificationView mbv = (NotificationView)sender;
                //MsgBoxService._rootControl.Children.Clear();
                NotificationService._MsgboxContainer.Children.Remove(mbv);
                if (NotificationService._MsgboxContainer.Children.Count == 0)
                    NotificationService._rootControl.Visibility = Visibility.Collapsed;
            })
            ;
            await NotificationService._rootControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);
        }
    }
}
