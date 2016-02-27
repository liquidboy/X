

using System;
using System.Threading.Tasks;
using SumoNinjaMonkey.Framework.Controls;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using SumoNinjaMonkey.Framework.Controls.Messages;
namespace SumoNinjaMonkey.Framework.Services
{
    public class InputBoxService
    {
        private static Grid _rootControl;
        private static StackPanel _MsgboxContainer;
        public static void Init(Grid control, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center)
        {
            InputBoxService._rootControl = control;
            _MsgboxContainer = new StackPanel();
            _MsgboxContainer.Orientation = Orientation.Vertical;
            _MsgboxContainer.HorizontalAlignment = horizontalAlignment;
            _MsgboxContainer.VerticalAlignment = verticalAlignment;
            _MsgboxContainer.ChildrenTransitions = new Windows.UI.Xaml.Media.Animation.TransitionCollection();
            _MsgboxContainer.ChildrenTransitions.Add(new Windows.UI.Xaml.Media.Animation.RepositionThemeTransition());
            _rootControl.Children.Add(_MsgboxContainer);


        }

        //double timeToLive,
        public async static void Show(
            string message, 
            string title,
            Brush modalAdornerBrush, 
            Brush backgroundBrush, 
            Brush foregroundTextBrush, 
            double width = 300, 
            double height = 180, 
            string metroIcon = "", 
            string imageIcon = "", 
            double scaleIcon = 1, 
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

            if (InputBoxService._rootControl != null)
            {
                DispatchedHandler invokedHandler = new DispatchedHandler(() =>
                {
                    if (InputBoxService._rootControl == null) //|| MsgBoxService._rootControl.Visibility == Visibility.Visible)
                    {
                        return;
                    }


                    //modal adorner
                    Rectangle rectModalAdorner = new Rectangle();
                    rectModalAdorner.Fill = modalAdornerBrush;
                    rectModalAdorner.Opacity = 0.4;
                    rectModalAdorner.HorizontalAlignment = HorizontalAlignment.Stretch;
                    rectModalAdorner.VerticalAlignment = VerticalAlignment.Stretch;
                    rectModalAdorner.SetValue(Canvas.ZIndexProperty, -2);
                    InputBoxService._rootControl.Children.Add(rectModalAdorner);


                    //message background
                    Rectangle rectBackground = new Rectangle();
                    rectBackground.Fill = backgroundBrush;
                    rectBackground.HorizontalAlignment = HorizontalAlignment.Stretch;
                    rectBackground.VerticalAlignment = VerticalAlignment.Center;
                    rectBackground.Height = height + 80;
                    rectBackground.SetValue(Canvas.ZIndexProperty, -1);
                    InputBoxService._rootControl.Children.Add(rectBackground);


                    InputBoxService._rootControl.Visibility = Visibility.Visible;

                    //message
                    InputBoxView msgBoxView = new InputBoxView(
                        message, 
                        "", 
                        foregroundTextBrush,
                        metroIcon : metroIcon,
                        imageIcon : imageIcon, 
                        scaleIcon : scaleIcon, 
                        translateXIcon : translateXIcon, 
                        translateYIcon : translateYIcon,
                        yesLabel : yesLabel,
                        yesMessengerContent : yesMessengerContent,
                        yesMessengerIdentifier : yesMessengerIdentifier,
                        noLabel : noLabel ,
                        noMessengerContent : noMessengerContent,
                        noMessengerIdentifier: noMessengerIdentifier,
                        msgToPassAlong: msgToPassAlong
                        );

                    msgBoxView.Width = width;
                    msgBoxView.Height = height;
                    msgBoxView.Margin = new Thickness(3);
                    msgBoxView.HorizontalAlignment =  HorizontalAlignment.Center;
                    msgBoxView.VerticalAlignment = VerticalAlignment.Center;
                    msgBoxView.BackgroundFill = backgroundBrush;
                    msgBoxView.MessageTextForegroundColor = foregroundTextBrush;
                    //msgBoxView.OnClosing += new EventHandler(MsgBoxService.view_OnClosing);
                    msgBoxView.Show();

                    InputBoxService._rootControl.Children.Add(msgBoxView);


                    //MsgBoxService._MsgboxContainer.Children.Insert(0, msgBoxView);

 
                });
                await InputBoxService._rootControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);

                
            
            }

        }

        public async static void Hide()
        {
            DispatchedHandler invokedHandler = new DispatchedHandler(() =>
            {
                InputBoxService._rootControl.Children.Clear();
                InputBoxService._rootControl.Visibility = Visibility.Collapsed;
            });
            await InputBoxService._rootControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);
        }


    }
}
