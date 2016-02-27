

using System;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls;
using SumoNinjaMonkey.Framework.Controls.Messages;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
namespace SumoNinjaMonkey.Framework.Services
{
    public class PopupService
    {
        private static bool _hasInitialized = false;
        private static Grid _rootControl;

        public static void Init(Grid control)
        {
            if (_hasInitialized) return;
            PopupService._rootControl = control;
            _hasInitialized = true;
        }

        public static void unload(){
            _rootControl = null;
            _hasInitialized = false;
        }


        public async static void Show(
            UserControl viewToShow, 
            UserControl toolbar,
            Brush accentPrimary,
            Brush accentSecondary,
            Brush foregroundTextBrush, 
            Brush countdownBackgroundBrush, 
            double timeToLive,
            Thickness margin,
            HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment verticalAlignment = VerticalAlignment.Center,
            bool autoHide = false, 
            double width = 300, 
            double height = 180, 
            string button1ClickContent = "",
            string button1ClickIdentifier = "", 
            string button2ClickContent = "",
            string button2ClickIdentifier = "", 
            SumoNinjaMonkey.Framework.Controls.PopupView.eCalloutAlign calloutAlign = PopupView.eCalloutAlign.None,
            string button1MetroIcon = "",
            double button1Rotation = 0,
            string button2MetroIcon = "",
            double button2Rotation = 0,
            bool showPopupInnerBorder = true
            )
        {
            if (PopupService._rootControl != null)
            {
                DispatchedHandler invokedHandler = new DispatchedHandler(() =>
                {
                    if (PopupService._rootControl == null )//|| PopupService._rootControl.Visibility == Visibility.Visible)
                    {
                        //Move(viewToShow, margin, calloutAlign);

                        return;
                    }
                    PopupService._rootControl.Visibility = Visibility.Visible;
                    PopupView view = new PopupView(
                        viewToShow,
                        accentPrimary,
                        accentSecondary,
                        autoHide, 
                        timeToLive, 
                        button1ClickContent,
                        button1ClickIdentifier,
                        button2ClickContent,
                        button2ClickIdentifier,
                        width: width, 
                        height: height, 
                        button1MetroIcon: button1MetroIcon, 
                        button1Rotation: button1Rotation,
                        button2MetroIcon: button2MetroIcon,
                        button2Rotation: button2Rotation,
                        toolbar : toolbar,
                        showInnerBorder: showPopupInnerBorder
                        );
                    view.ContentThickness = new Thickness(0, 0, 0, 0);
                    view.HorizontalAlignment = horizontalAlignment;
                    view.VerticalAlignment = verticalAlignment;
                    //view.Margin = margin;
                    view.BackgroundFill = accentPrimary;
                    view.MessageTextForegroundColor = foregroundTextBrush;
                    view.CountdownBackgroundColor = countdownBackgroundBrush;
                    view.Show(margin.Left, margin.Top);
                    view.CalloutAlign = calloutAlign;
                    view.OnClosing += new EventHandler(PopupService.view_OnClosing);

                    
                    PopupService._rootControl.Children.Add(view);
                    
                });
                await PopupService._rootControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);
            }
        }


        private async static void Move(
           UserControl viewToMove,
           Thickness margin,
           SumoNinjaMonkey.Framework.Controls.PopupView.eCalloutAlign calloutAlign
            )
        {
            foreach (var child in PopupService._rootControl.Children)
            {
                if (child.GetType().Name ==  "PopupView")
                {
                    PopupView pv = (PopupView)child;
                    if (pv.MainContent.GetType().Name == viewToMove.GetType().Name)
                    {
                        //pv.Margin = margin;
                        pv.Move(margin.Left, margin.Top);
                        pv.CalloutAlign = calloutAlign;
                        return;
                    };
                }
            }
        }

        public async static void ToogleToolbar(UserControl viewToToggle)
        {
            foreach (var child in PopupService._rootControl.Children)
            {
                if (child.GetType().Name == "PopupView")
                {
                    PopupView pv = (PopupView)child;
                    if (pv.MainContent.GetType().Name == viewToToggle.GetType().Name)
                    {
                        pv.ToogleToolbar();
                        return;
                    };
                }
            }
        }

        public static bool IsToolbarVisible(UserControl viewToInterogate)
        {
            bool ret = false;

            foreach (var child in PopupService._rootControl.Children)
            {
                if (child.GetType().Name == "PopupView")
                {
                    PopupView pv = (PopupView)child;
                    if (pv.MainContent.GetType().Name == viewToInterogate.GetType().Name)
                    {
                        return pv.ToolbarIsVisible;
                    };
                }
            }

            return ret;
        }

        private async static void view_OnClosing(object sender, EventArgs e)
        {
            DispatchedHandler invokedHandler = new DispatchedHandler(() => {
                PopupService._rootControl.Children.Remove((UIElement)sender);
                if(PopupService._rootControl.Children.Count == 0)
                    PopupService._rootControl.Visibility = Visibility.Collapsed;
            })
            ;
            await PopupService._rootControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);
        }

        public async static void Close(UserControl viewToClose)
        {
            foreach (var child in PopupService._rootControl.Children)
            {
                if (child.GetType().Name == "PopupView")
                {
                    PopupView pv = (PopupView)child;
                    if (pv.MainContent.GetType().Name == viewToClose.GetType().Name)
                    {
                        pv.Hide();
                        return;
                    };
                }
            }
        }

        public async static void CloseAll()
        {
            foreach (var child in PopupService._rootControl.Children)
            {
                if (child.GetType().Name == "PopupView")
                {
                    PopupView pv = (PopupView)child;
                    pv.Hide();
                }
            }
        }


        public static bool HasPopup { 
            get {
                bool _val = false;

                if (_rootControl != null)
                {
                    foreach (var child in PopupService._rootControl.Children)
                    {
                        if (child.GetType().Name == "PopupView")
                        {
                            _val = true;
                            break;
                        }
                    }

                    return _val;
                }
                else return _val;
            } 

        }


    }
}
