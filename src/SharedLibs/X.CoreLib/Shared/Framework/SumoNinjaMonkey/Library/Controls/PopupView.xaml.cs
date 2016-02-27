using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SumoNinjaMonkey.Framework.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class PopupView : UserControl
    {
        public event EventHandler OnClosing;
        private DispatcherTimer dtClose;

        private string _button1ClickContent = string.Empty;
        private string _button2ClickContent = string.Empty;
        private string _button1ClickIdentifier = string.Empty;
        private string _button2ClickIdentifier = string.Empty;

        public enum eCalloutAlign
        {
            Left,
            Right,
            None
        }


        private eCalloutAlign _calloutAlign = eCalloutAlign.None;
        public eCalloutAlign CalloutAlign { 
            get { return _calloutAlign; } 
            set { 
                _calloutAlign = value;
                if (_calloutAlign == eCalloutAlign.Left)
                    pthCallout.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                else if (_calloutAlign == eCalloutAlign.Right)
                    pthCallout.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                else
                {
                    pthCallout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    recInnerBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }

                
            }
        }


        public Brush BackgroundFill
        {
            get { return (Brush)GetValue(BackgroundFillProperty); }
            set { SetValue(BackgroundFillProperty, value); }
        }

        
        public static readonly DependencyProperty BackgroundFillProperty =
            DependencyProperty.Register("BackgroundFill", typeof(Brush), typeof(PopupView), new PropertyMetadata(0, BackgroundFillPropertyChanged));

        private static void BackgroundFillPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PopupView ctl = (PopupView)obj;

            ctl.recBackground.Fill = (Brush)args.NewValue;
            ctl.pthCallout.Fill = (Brush)args.NewValue;
        }


        public Brush InnerBorderColor
        {
            get { return (Brush)GetValue(InnerBorderColorProperty); }
            set { SetValue(InnerBorderColorProperty, value); }
        }


        public static readonly DependencyProperty InnerBorderColorProperty =
            DependencyProperty.Register("InnerBorderColor", typeof(Brush), typeof(PopupView), new PropertyMetadata(0, InnerBorderColorPropertyChanged));

        private static void InnerBorderColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PopupView ctl = (PopupView)obj;

            ctl.recInnerBorder.Fill = (Brush)args.NewValue;

        }



        public Brush MessageTextForegroundColor
        {
            get { return (Brush)GetValue(MessageTextForegroundColorProperty); }
            set { SetValue(MessageTextForegroundColorProperty, value); }
        }


        public static readonly DependencyProperty MessageTextForegroundColorProperty =
            DependencyProperty.Register("MessageTextForegroundColor", typeof(Brush), typeof(PopupView), new PropertyMetadata(0, MessageTextForegroundColorPropertyChanged));

        private static void MessageTextForegroundColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PopupView ctl = (PopupView)obj;

            

        }

        public Brush CountdownBackgroundColor
        {
            get { return (Brush)GetValue(CountdownBackgroundColorProperty); }
            set { SetValue(CountdownBackgroundColorProperty, value); }
        }


        public static readonly DependencyProperty CountdownBackgroundColorProperty =
            DependencyProperty.Register("CountdownBackgroundColor", typeof(Brush), typeof(PopupView), new PropertyMetadata(0, CountdownBackgroundColorPropertyChanged));

        private static void CountdownBackgroundColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PopupView ctl = (PopupView)obj;

            ctl.recTimeLeft.Fill = (Brush)args.NewValue;
            ctl.recTimeLeft.Stroke = (Brush)args.NewValue;

        }



        public Thickness ContentThickness
        {
            get { return (Thickness)GetValue(ContentThicknessProperty); }
            set { SetValue(ContentThicknessProperty, value); }
        }

        public static readonly DependencyProperty ContentThicknessProperty =
            DependencyProperty.Register("ContentThickness", typeof(Thickness), typeof(PopupView), new PropertyMetadata(new Thickness(10d,10d,10d,30d), ContentThicknessPropertyChanged));

        private static void ContentThicknessPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PopupView pv = (PopupView)obj;
            pv.grdCustomControl.Margin = (Thickness)args.NewValue;
        }


        public void Move(double newX, double newY)
        {

            ((EasingDoubleKeyFrame)((DoubleAnimationUsingKeyFrames)sbMove.Children[0]).KeyFrames[0]).Value = newX;
            ((EasingDoubleKeyFrame)((DoubleAnimationUsingKeyFrames)sbMove.Children[1]).KeyFrames[0]).Value = newY;
            sbMove.Begin();

            //MsgBoxService.Show(moveX.ToString(), "");

        }


        public void Show(double newX, double newY)
        {
            
            ((EasingDoubleKeyFrame)((DoubleAnimationUsingKeyFrames)sbShow.Children[1]).KeyFrames[0]).Value = newX;
            ((EasingDoubleKeyFrame)((DoubleAnimationUsingKeyFrames)sbShow.Children[2]).KeyFrames[0]).Value = -45; //newY;

            ((CompositeTransform)layoutRoot.RenderTransform).TranslateX = newX;

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
                dtClose = null;

                grdCustomControl.Children.Clear();
                ToolbarContent = null;
                grdToolbar.Children.Clear();
                MainContent = null;
            };
        }


        public void ShowToolbar()
        {
            sbShowToolbar.Begin();
        }


        public void HideToolbar()
        {
            sbHideToolbar.Begin();
        }

        public void ToogleToolbar()
        {
            if (grdToolbar.Opacity == 0) ShowToolbar();
            else HideToolbar();
        }

        public bool ToolbarIsVisible
        {
            get
            {
                if (grdToolbar.Opacity == 0) return false;
                else return true;
            }

        }

        public UserControl MainContent { get; set; }
        public UserControl ToolbarContent { get; set; }


        public PopupView(
            UserControl viewToLoad, 
            Brush primaryAccent,
            Brush secondaryAccent,
            bool autoHide, 
            double timeToLive,
            string button1ClickContent = "",
            string button1ClickIdentifier = "",
            string button2ClickContent = "",
            string button2ClickIdentifier = "", 
            double width = 100, 
            double height = 100, 
            string button1MetroIcon = "",
            double button1Rotation = 0,
            string button2MetroIcon = "",
            double button2Rotation = 0,
            UserControl toolbar = null,
            bool showInnerBorder = true
            )
        {
            this.InitializeComponent();

            if (viewToLoad == null) return;

            _button1ClickContent = button1ClickContent;
            _button1ClickIdentifier = button1ClickIdentifier;
            butTopRight1.ClickCode = _button1ClickContent;
            butTopRight1.ClickIdentifier = _button1ClickIdentifier;
            butTopRight1.Visibility = (string.IsNullOrEmpty(_button1ClickContent) || string.IsNullOrEmpty(button1MetroIcon)) ? Visibility.Collapsed : Visibility.Visible;
            butTopRight1.UpdateBackgroundColor(primaryAccent);
            if (!string.IsNullOrEmpty(button1MetroIcon)) butTopRight1.LoadMetroIcon(button1MetroIcon, rotation: button1Rotation);



            _button2ClickContent = button2ClickContent;
            _button2ClickIdentifier = button2ClickIdentifier;
            butTopRight2.ClickCode = _button2ClickContent;
            butTopRight2.ClickIdentifier = _button2ClickIdentifier;
            butTopRight2.Visibility = (string.IsNullOrEmpty(_button2ClickContent) || string.IsNullOrEmpty(button2MetroIcon)) ? Visibility.Collapsed : Visibility.Visible;
            butTopRight2.UpdateBackgroundColor(primaryAccent);
            if (!string.IsNullOrEmpty(button2MetroIcon)) butTopRight2.LoadMetroIcon(button2MetroIcon, rotation: button2Rotation);
            
                    

            
            if (this.MainContent == null)
            {
                this.MainContent = viewToLoad;
                this.grdCustomControl.Children.Add(viewToLoad);
            }

            if (this.ToolbarContent == null && toolbar != null)
            {
                this.ToolbarContent = toolbar;
                this.grdToolbar.Children.Add(toolbar);
            }

            if (showInnerBorder) recInnerBorder.Visibility = Visibility.Visible;
            else recInnerBorder.Visibility = Visibility.Collapsed;

            this.Width = width;
            this.Height = height;
            //ggClip.Rect = new Rect(0, 0, width-20, height-40);

            ((DoubleAnimation)sbCountdown.Children[0]).Duration = new Duration(TimeSpan.FromSeconds(timeToLive));

            if (timeToLive < 999)
            {
                recTimeLeft.Visibility = Visibility.Visible;
                dtClose = new DispatcherTimer();
                dtClose.Interval = TimeSpan.FromSeconds(timeToLive);
                dtClose.Tick += (o, e) =>
                {
                    //sbCountdown.Stop(); 
                    dtClose.Stop();
                    this.Hide();
                };
                dtClose.Start();
            }
            else
            {
                recTimeLeft.Visibility = Visibility.Collapsed;
            }
        }


        
    }
}
